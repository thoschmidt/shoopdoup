/////////////////////////////////////////////////////////////////////////
//
// This module provides sample code used to demonstrate the use
// of the KinectAudioSource for audio capture and beam tracking
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is licensed under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) 
// License Agreement: http://kinectforwindows.org/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Research.Kinect.Audio;

namespace RecordAudio
{
    class Program
    {
        static void Main(string[] args)
        {
            var buffer = new byte[4096];
            const int recordTime = 20; //seconds
            const int recordingLength = recordTime * 2 * 16000; //10 seconds, 16 bits per sample, 16khz
            const string outputFileName = "out.wav";
            
            //We need to run in high priority to avoid dropping samples 
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            //Instantiate the KinectAudioSource to do audio capture
            using (var source = new KinectAudioSource())
            {
		        source.SystemMode = SystemMode.OptibeamArrayOnly;

                //Register for beam tracking change notifications
                source.BeamChanged += source_BeamChanged;

                using (var fileStream = new FileStream(outputFileName, FileMode.Create))
                {
                    using (var sampleStream = new StreamWriter(new FileStream("samples.log", FileMode.Create)))
                    {
                        WriteWavHeader(fileStream, recordingLength);

                        Console.WriteLine("Recording for {0} seconds", recordTime);

                        //Start capturing audio                               
                        using (var audioStream = source.Start())
                        {
                            //Simply copy the data from the stream down to the file
                            int count, totalCount = 0;
                            while ((count = audioStream.Read(buffer, 0, buffer.Length)) > 0 && totalCount < recordingLength)
                            {
                                for (int i = 0; i < buffer.Length; i += 2)
                                {
                                    short sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                                    sampleStream.WriteLine(sample);
                                }

                                fileStream.Write(buffer, 0, count);
                                totalCount += count;

                                //If we have high confidence, print the position
                                if (source.SoundSourcePositionConfidence > 0.9)
                                    Console.Write("Sound source position (radians): {0}\t\tBeam: {1}\r", source.SoundSourcePosition, source.MicArrayBeamAngle);
                            }
                        }
                    }
                }

                Console.WriteLine("Recording saved to {0}", outputFileName);
            }
        }

        static void source_BeamChanged(object sender, BeamChangedEventArgs e)
        {
            Console.WriteLine("\nBeam direction changed (radians): {0}", e.Angle);
        }



        /// <summary>
        /// A bare bones WAV file header writer
        /// </summary>        
        static void WriteWavHeader(Stream stream, int dataLength)
        {           
            //We need to use a memory stream because the BinaryWriter will close the underlying stream when it is closed
			using(MemoryStream memStream = new MemoryStream(64))
			{
				int cbFormat = 18; //sizeof(WAVEFORMATEX)
				WAVEFORMATEX format = new WAVEFORMATEX()
										  {
											  wFormatTag = 1,
											  nChannels = 1,
											  nSamplesPerSec = 16000,
											  nAvgBytesPerSec = 32000,
											  nBlockAlign = 2,
											  wBitsPerSample = 16,
											  cbSize = 0
										  };
	
				using (var bw = new BinaryWriter(memStream))
				{   
					//RIFF header
					WriteString(memStream, "RIFF");
					bw.Write(dataLength + cbFormat +  4 ); //File size - 8
					WriteString(memStream, "WAVE");                
					WriteString(memStream, "fmt ");
					bw.Write(cbFormat);
	
					//WAVEFORMATEX
					bw.Write(format.wFormatTag);
					bw.Write(format.nChannels);
					bw.Write(format.nSamplesPerSec);
					bw.Write(format.nAvgBytesPerSec);
					bw.Write(format.nBlockAlign);
					bw.Write(format.wBitsPerSample);
					bw.Write(format.cbSize);
	
					//data header
					WriteString(memStream, "data");					
					bw.Write(dataLength);
				    memStream.WriteTo(stream);
				}
            }                      
        }

        static void WriteString(Stream stream, string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            Debug.Assert(bytes.Length==s.Length);
            stream.Write(bytes, 0 , bytes.Length);
        }

        struct WAVEFORMATEX
        {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }
    }
}
