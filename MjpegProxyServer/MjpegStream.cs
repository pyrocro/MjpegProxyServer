using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AForge.Video;
using System.Diagnostics;
using System.Collections.Generic;

namespace MjpegProxyServer
{
    public class MjpegStream
    {
        public Dictionary<string, MjpegWebSocketBehavior> connectionList = new Dictionary<string, MjpegWebSocketBehavior>();
        bool running = false;
        public bool RUNNING { get { return running; } }        
        
        public bool hasClients()
        {
            if (connectionList.Count <= 0) return false;
            return true;
        }
        Stopwatch watch = new Stopwatch();
        int fpsCount = 0;
        public double streamFPS = 0;

        /// <summary>
        /// The max images in queue.
        /// </summary>
        public const int MAX_IMAGES_IN_QUEUE = 15;

		/// <summary>
		/// The image queue.
		/// </summary>
		Queue<FrameData> imageQueue = new Queue<FrameData>();

		/// <summary>
		/// The stream.
		/// </summary>
		private MJPEGStream stream = null;//new MJPEGStream("http://ymc.noip.me:8081");

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MjpegProxyServer.MjpegStreamManager"/> class.
		/// </summary>
		/// <param name="url">URL.</param>
		public MjpegStream(string url)
		{
			stream = new MJPEGStream(url);
            watch.Start();
            // set event handlers
            stream.NewFrame += Stream_NewFrame;//new NewFrameEventHandler(video_NewFrame);
            stream.VideoSourceError += Stream_VideoSourceError;
        }

		/// <summary>
		/// Gets the queue count.
		/// </summary>
		/// <value>The queue count.</value>
		public int QueueCount
		{
			get { return imageQueue.Count;}
		}

		/// <summary>
		/// Gets the current image.
		/// </summary>
		/// <value>The current image.</value>
		public FrameData currentImage{
			get
			{
				if (imageQueue.Count == 0) return new FrameData(new Bitmap(640,480));
                FrameData peek = null;
                lock (imageQueue)
                {
                    peek = imageQueue.Peek();
                }
                return peek;//imageQueue.Dequeue().ToString();
			}
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public void start()
		{            
			
			// start the video source
			stream.Start();
			Console.WriteLine("Started Mjpeg Stream connection...........");
            //var sr = new ServerRequest();
            //string s = Newtonsoft.Json.JsonConvert.SerializeObject(sr);
            if (watch != null) watch.Stop();
            fpsCount = 0;
            streamFPS = 0;
            watch = new Stopwatch();
            watch.Start();
            running = true;

        }

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void stop()
		{
            if (stream != null)
            {
                stream.SignalToStop();
                stream.WaitForStop();
                //stream.Stop();
            }
            if (watch != null)
            {
                watch.Reset();
                watch.Stop();
            }
            running = false;
        }
        public bool isRunning()
        {
            if (stream == null) return false;
            return stream.IsRunning;
        }

		/// <summary>
		/// Streams the video source error.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void Stream_VideoSourceError(object sender, VideoSourceErrorEventArgs eventArgs)
		{
			Console.WriteLine("Error....." + eventArgs.Description);
		}

		/// <summary>
		/// Streams the new frame.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void Stream_NewFrame(object sender, NewFrameEventArgs eventArgs) 
		{
            //Console.Write("\t(" + imageQueue.Count+"-r"+this.stream.BytesReceived+") ");

            FrameData frameData = new FrameData(eventArgs.Frame);
            //string base64ConvertedString = ImageToBase64(eventArgs.Frame, System.Drawing.Imaging.ImageFormat.Jpeg);
            eventArgs.Frame.Dispose();

            if (imageQueue.Count > MAX_IMAGES_IN_QUEUE)
			{
                lock (imageQueue)
                {
                    imageQueue.Dequeue(); //remove 1st/oldest frame from the queue
                }
			}
            fpsCount++;
            if (watch.Elapsed.TotalMilliseconds >= 1000)
            {
                this.streamFPS = Math.Round(fpsCount/(watch.Elapsed.TotalMilliseconds/1000),2);
                fpsCount = 0;
                watch.Reset();
                watch.Restart();
            }
            imageQueue.Enqueue(frameData);
			//Console.Write("\t("+eventArgs.Frame.Width + "x" + eventArgs.Frame.Height + "x" + eventArgs.Frame.PixelFormat.ToString()+")"+imageQueue.Count+"\t ");
			//Console.WriteLine();
		}


       
    }
}
