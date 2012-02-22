using System;
using System.Collections.Generic;
using System.Collections;
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
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;
using MySql.Data.MySqlClient;
using ShoopDoup;

namespace SkeletalTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        //Kinect Runtime
        Runtime nui;

        //Targets and skeleton controller
        ShoopDoupController shoopDoupController;
        SelectionController selectionController;

        DtwGestureRecognizer _dtw = new DtwGestureRecognizer(12, 0.6, 2, 2, 10);
        ArrayList _video = new ArrayList();
        private int _totalFrames;
        private int _flipFlop;
        private int _lastFrames;
        private DateTime _lastTime = DateTime.MaxValue;
        private const int BufferSize = 32;
        private const int MinimumFrames = 6;
        private const int Ignore = 2;

        //SQL stuff
        
        Dictionary<String, String> movieDatabase;


        //Holds the currently active controller
        SkeletonController currentController;

        ImageManager imageManager;
      
        //Scaling constants
        public float k_xMaxJointScale = 1.5f;
        public float k_yMaxJointScale = 1.5f;

        int i;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupKinect();
            shoopDoupController = new ShoopDoupController(this);
            selectionController = new SelectionController(this);
            currentController = shoopDoupController;
        
            
            i = 0;
        }

        private void SetupKinect()
        {
            if (Runtime.Kinects.Count == 0)
            {
                this.Title = "No Kinect connected"; 
            }
            else
            {
                //use first Kinect
                nui = Runtime.Kinects[0];

                //Initialize to do skeletal tracking
                nui.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor | RuntimeOptions.UseDepthAndPlayerIndex);

                //add event to receive skeleton data
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
                nui.SkeletonFrameReady += NuiSkeletonFrameReady;
                nui.SkeletonFrameReady += SkeletonExtractSkeletonFrameReady;

                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReady;

                //add event to receive video data
                nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_VideoFrameReady);

                //to experiment, toggle TransformSmooth between true & false and play with parameters            
                nui.SkeletonEngine.TransformSmooth = true;
                TransformSmoothParameters parameters = new TransformSmoothParameters();
                // parameters used to smooth the skeleton data
                parameters.Smoothing = 0.3f;
                parameters.Correction = 0.3f;
                parameters.Prediction = 0.4f;
                parameters.JitterRadius = 0.7f;
                parameters.MaxDeviationRadius = 0.2f;
                nui.SkeletonEngine.SmoothParameters = parameters;

                //Open the video stream
                nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);

                LoadGesturesFromFile(System.AppDomain.CurrentDomain.BaseDirectory + "RightHandSwipeLeft.txt");
                LoadGesturesFromFile(System.AppDomain.CurrentDomain.BaseDirectory + "RightHandSwipeRight.txt");
                
                //Force video to the background
                Canvas.SetZIndex(image1, -10000);
            }
        }

        void nui_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            //Automagically create BitmapSource for Video
            image1.Source = e.ImageFrame.ToBitmapSource();            
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            
            SkeletonFrame allSkeletons = e.SkeletonFrame;

            //get the first tracked skeleton
            SkeletonData skeleton = (from s in allSkeletons.Skeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();


            if(skeleton != null)
            {
                //set positions on our joints of interest (already defined as Ellipse objects in the xaml)

                SetImagePosition(hand, skeleton.Joints[JointID.HandRight]);
                currentController.processSkeletonFrame(skeleton, null);

            }
        }

        private void NuiSkeleton2DdataCoordReady(object sender, Skeleton2DdataCoordEventArgs a)
        {
            // We need a sensible number of frames before we start attempting to match gestures against remembered sequences
            if (_video.Count > MinimumFrames)
            {
                ////Debug.WriteLine("Reading and video.Count=" + video.Count);
                string s = _dtw.Recognize(_video);

                //results.Text = "Recognised as: " + s;
                if (!s.Contains("__UNKNOWN"))
                {
                    // There was no match so reset the buffer
                    Console.WriteLine("Found " + s);
                    ((ShoopDoupController)currentController).resetTargets();
                    _video = new ArrayList();
                }
            }

            // Ensures that we remember only the last x frames
            if (_video.Count > BufferSize)
            {
                    // Remove the first frame in the buffer
                    _video.RemoveAt(0);
            }

            // Decide which skeleton frames to capture. Only do so if the frames actually returned a number. 
            // For some reason my Kinect/PC setup didn't always return a double in range (i.e. infinity) even when standing completely within the frame.
            // TODO Weird. Need to investigate this
            if (!double.IsNaN(a.GetPoint(0).X))
            {
                // Optionally register only 1 frame out of every n
                _flipFlop = (_flipFlop + 1) % Ignore;
                if (_flipFlop == 0)
                {
                    _video.Add(a.GetCoords());
                }
            }

            // Update the debug window with Sequences information
            //dtwTextOutput.Text = _dtw.RetrieveText();
        }

        private void SetEllipsePosition(Ellipse ellipse, Joint joint)
        {    
            var scaledJoint = joint.ScaleTo(640, 480, k_xMaxJointScale, k_yMaxJointScale);

            Canvas.SetLeft(ellipse, scaledJoint.Position.X - (double)ellipse.GetValue(Canvas.WidthProperty) / 2 );
            Canvas.SetTop(ellipse, scaledJoint.Position.Y - (double)ellipse.GetValue(Canvas.WidthProperty) / 2);
            Canvas.SetZIndex(ellipse, (int) -Math.Floor(scaledJoint.Position.Z*100));
            if (joint.ID == JointID.HandLeft || joint.ID == JointID.HandRight)
            {   
                byte val = (byte)(Math.Floor((joint.Position.Z - 0.8)* 255 / 2));
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(val, val, val));
            }
        }

        private void SetImagePosition(Image image, Joint joint)
        {
            var scaledJoint = joint.ScaleTo(640, 480, k_xMaxJointScale, k_yMaxJointScale);

            Canvas.SetLeft(image, scaledJoint.Position.X - (double)image.GetValue(Canvas.WidthProperty) / 2);
            Canvas.SetTop(image, scaledJoint.Position.Y - (double)image.GetValue(Canvas.WidthProperty) / 2);
            Canvas.SetZIndex(image, (int)-Math.Floor(scaledJoint.Position.Z * 100));
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            //Cleanup
            nui.Uninitialize();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1)
            {
                currentController = shoopDoupController;
            }

            if (e.Key == Key.D2)
            {
                currentController = selectionController;
            }
        }

        private static void SkeletonExtractSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                Skeleton2DDataExtract.ProcessData(data);
            }
        }

        /// <summary>
        /// Runds every time a skeleton frame is ready. Updates the skeleton canvas with new joint and polyline locations.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Skeleton Frame Event Args</param>
        private void NuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            int iSkeleton = 0;
            var brushes = new Brush[6];
            brushes[0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            brushes[1] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            brushes[2] = new SolidColorBrush(Color.FromRgb(64, 255, 255));
            brushes[3] = new SolidColorBrush(Color.FromRgb(255, 255, 64));
            brushes[4] = new SolidColorBrush(Color.FromRgb(255, 64, 255));
            brushes[5] = new SolidColorBrush(Color.FromRgb(128, 128, 255));

        }

        /// <summary>
        /// Called every time a video (RGB) frame is ready
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Image Frame Ready Event Args</param>
        private void NuiColorFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            // 32-bit per pixel, RGBA image
            PlanarImage image = e.ImageFrame.Image;
        }


        //from DTW
        /// <summary>
        /// Opens the sent text file and creates a _dtw recorded gesture sequence
        /// Currently not very flexible and totally intolerant of errors.
        /// </summary>
        /// <param name="fileLocation">Full path to the gesture file</param>
        public void LoadGesturesFromFile(string fileLocation)
        {
            int itemCount = 0;
            string line;
            string gestureName = String.Empty;

            // TODO I'm defaulting this to 12 here for now as it meets my current need but I need to cater for variable lengths in the future
            ArrayList frames = new ArrayList();
            double[] items = new double[12];

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(fileLocation);
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("@"))
                {
                    gestureName = line;
                    continue;
                }

                if (line.StartsWith("~"))
                {
                    frames.Add(items);
                    itemCount = 0;
                    items = new double[12];
                    continue;
                }

                if (!line.StartsWith("----"))
                {
                    items[itemCount] = Double.Parse(line);
                }

                itemCount++;

                if (line.StartsWith("----"))
                {
                    _dtw.AddOrUpdate(frames, gestureName);
                    frames = new ArrayList();
                    gestureName = String.Empty;
                    itemCount = 0;
                }
            }

            file.Close();
        }

        private void imageTarget1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }


}
