using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.Research.Kinect.Audio;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace KinectAudioDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double alpha = 0.35;
        private readonly SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
        private readonly SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush blueBrush = new SolidColorBrush(Colors.Blue);
        private readonly SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);

        private KinectAudioSource kinectSource;        
        private double angle;        
        private bool running = true;
        private SpeechRecognitionEngine sre;
        private readonly WriteableBitmap bmWav;
        private EnergyCalculatingPassThroughStream stream;
        private byte[] pixels;
        private const int WavImageWidth = 500;
        private const int WavImageHeight = 100;
        private double[] energyBuffer  = new double[WavImageWidth];
        private byte[] blackPixels = new byte[WavImageWidth * WavImageHeight];
        private Int32Rect fullImageRect = new Int32Rect(0,0,WavImageWidth,WavImageHeight);

        public MainWindow()
        {
            InitializeComponent();

            var colors = new List<System.Windows.Media.Color>();
            colors.Add(Colors.Black);
            colors.Add(Colors.Green);            
            bmWav = new WriteableBitmap(WavImageWidth, WavImageHeight, 96, 96, PixelFormats.Indexed1, new BitmapPalette(colors));

            pixels = new byte[WavImageWidth];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = 0xff;

            imgWav.Source = bmWav;
        }

        void MainWindow_Loaded(object sender, EventArgs e)
        {
            InitializeSpeechRecognition();

            Start();
            Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
            kinectSource.Stop();
            sre.RecognizeAsyncCancel();
            sre.RecognizeAsyncStop();
            kinectSource.Dispose();
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private void InitializeSpeechRecognition()
        {
            RecognizerInfo ri = GetKinectRecognizer();
            if (ri == null)
            {
                MessageBox.Show(
                    @"There was a problem initializing Speech Recognition.
Ensure you have the Microsoft Speech SDK installed.",
                    "Failed to load Speech SDK",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }

            try
            {
                sre = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                MessageBox.Show(
                    @"There was a problem initializing Speech Recognition.
Ensure you have the Microsoft Speech SDK installed and configured.",
                    "Failed to load Speech SDK",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }

            var colors = new Choices();
            colors.Add("red");
            colors.Add("green");
            colors.Add("blue");

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(colors);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);
            

            sre.LoadGrammar(g);
            sre.SpeechRecognized += sre_SpeechRecognized;
            sre.SpeechHypothesized += sre_SpeechHypothesized;
            sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);

        }

        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {            
            ReportSpeechStatus("Rejected: " + (e.Result==null? string.Empty : e.Result.Text + " " + e.Result.Confidence));

            Dispatcher.BeginInvoke(new Action(() =>
            {
                tbColor.Background = blackBrush;
            }), DispatcherPriority.Normal);
        }


        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            ReportSpeechStatus("Hypothesized: " + e.Result.Text + " " + e.Result.Confidence);
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SolidColorBrush brush;

            if (e.Result.Confidence < 0.8)
                return;

            switch (e.Result.Text.ToUpperInvariant())
            {
                case "RED":
                    brush = redBrush;
                    break;
                case "GREEN":
                    brush = greenBrush;
                    break;
                case "BLUE":
                    brush = blueBrush;
                    break;
                default:
                    brush = blackBrush;
                    break;
            }

            ReportSpeechStatus("Recognized: "+e.Result.Text + " " + e.Result.Confidence);
            Dispatcher.BeginInvoke(new Action(() =>
                                                  {                                                      
                                                      tbColor.Background = brush;
                                                  }), DispatcherPriority.Normal);

        }

        private void ReportSpeechStatus(string status)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                                                  {
                                                      tbSpeechStatus.Text = status;
                                                  }), DispatcherPriority.Normal);
        }

        private void Start()
        {
            try
            {
                kinectSource = new KinectAudioSource();
                kinectSource.SystemMode = SystemMode.OptibeamArrayOnly;
                kinectSource.FeatureMode = true;
                kinectSource.AutomaticGainControl = false;
                //kinectSource.Noi
                kinectSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
                var kinectStream = kinectSource.Start();
                stream = new EnergyCalculatingPassThroughStream(kinectStream);
                sre.SetInputToAudioStream(stream, new SpeechAudioFormatInfo(
                                                      EncodingFormat.Pcm, 16000, 16, 1,
                                                      32000, 2, null));
                sre.RecognizeAsync(RecognizeMode.Multiple);
                var t = new Thread(PollSoundSourceLocalization);
                t.Start();
            }
            catch
            {
                MessageBox.Show(
                    @"There was a problem initializing the KinectAudioSource.
Ensure you have the Kinect SDK installed correctly.", 
                    "Failed to load Speech SDK",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
        }

        private void PollSoundSourceLocalization()
        {
            while (running)
            {                                

                if (kinectSource.SoundSourcePositionConfidence > 0.5)
                {
                    //Smooth the change in angle
                    double a = alpha*kinectSource.SoundSourcePositionConfidence;
                    angle = (1 - a)*angle + a*kinectSource.SoundSourcePosition;

                    Dispatcher.BeginInvoke(new Action(() =>
                                                          {
                                                              rotTx.Angle = -(angle/Math.PI)*180;                                                              
                                                          }), DispatcherPriority.Normal);



                }

                Dispatcher.BeginInvoke(new Action(() =>
                                                      {
                                                          clipConf.Rect = new Rect(0,0, 100 + 600 * kinectSource.SoundSourcePositionConfidence, 50);
                                                          string sConf = string.Format("Conf: {0:0.00}",
                                                                                      kinectSource.
                                                                                          SoundSourcePositionConfidence);
                                                          tbConf.Text = sConf;
                                                          
                                                          stream.GetEnergy(energyBuffer);
                                                          bmWav.WritePixels(fullImageRect, blackPixels, WavImageWidth, 0);
                                                          
                                                          for (int i = 1; i < energyBuffer.Length; i++)
                                                          {
                                                              int energy = (int)(energyBuffer[i]*5);
                                                              Int32Rect r = new Int32Rect(i, WavImageHeight/2 - energy, 1, 2 * energy);
                                                              bmWav.WritePixels(r, pixels, 1, 0);
                                                          }

                                                      }), DispatcherPriority.Normal);


                Thread.Sleep(50);
            }
        }

        class EnergyCalculatingPassThroughStream : Stream
        {
            private readonly Stream baseStream;
            private int index = 0;
            private readonly double[] energy = new double[WavImageWidth];
            private readonly object syncRoot = new object();
            const int samplesPerPixel = 10;
            int sampleCount = 0;
            double avgSample = 0;

            public EnergyCalculatingPassThroughStream (Stream stream)
            {
                baseStream = stream;
            }

            public override bool CanRead
            {
                get { return baseStream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return baseStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return baseStream.CanWrite; }
            }

            public override void Flush()
            {
                baseStream.Flush();
            }

            public override long Length
            {
                get { return baseStream.Length; }
            }

            public override long Position
            {
                get { return baseStream.Position; }
                set { baseStream.Position = value; }
            }

            public void GetEnergy(double[] energyBuffer)
            {
                lock (syncRoot)
                {
                    int energyIndex = index;
                    for (int i = 0; i < energy.Length; i++)
                    {
                        energyBuffer[i] = energy[energyIndex];                        
                        energyIndex++;
                        if (energyIndex >= energy.Length)
                            energyIndex = 0;
                        
                    }
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {                
                int retVal = baseStream.Read(buffer, offset, count);
                double a = 0.3;
                lock (syncRoot)
                {
                    for (int i = 0; i < retVal; i += 2)
                    {
                       
                        short sample = BitConverter.ToInt16(buffer, i + offset);
                        avgSample += sample*sample;
                        sampleCount++;

                        if (sampleCount == samplesPerPixel)
                        {
                            avgSample /= samplesPerPixel;

                            energy[index] = .2+(avgSample*11)/(int.MaxValue/2); //2^30 = (2^15)^2
                            energy[index] = energy[index] > 10 ? 10 : energy[index];

                            if (index > 0)
                                energy[index] = energy[index]*a + (1 - a)*energy[index - 1];

                            index++;
                            if (index >= energy.Length)
                                index = 0;
                            avgSample = 0;
                            sampleCount = 0;
                        }
                        
                    }
                }

                return retVal;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return baseStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                baseStream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                baseStream.Write(buffer, offset, count);
            }
        }

    }
}
