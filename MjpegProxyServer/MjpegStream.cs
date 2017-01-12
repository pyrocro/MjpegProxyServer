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
        public const int MAX_IMAGES_IN_QUEUE = 60;

		/// <summary>
		/// The image queue.
		/// </summary>
		Queue imageQueue = new Queue();

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
		public string currentImage{
			get
			{
				if (imageQueue.Count == 0) return "";
				return imageQueue.Peek().ToString();//imageQueue.Dequeue().ToString();
			}
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public void start()
		{
			
			// set event handlers
			stream.NewFrame += Stream_NewFrame;//new NewFrameEventHandler(video_NewFrame);
			stream.VideoSourceError += Stream_VideoSourceError;
			// start the video source
			stream.Start();            
			Console.WriteLine("Started Mjpeg Stream connection...........");
			//var sr = new ServerRequest();
			//string s = Newtonsoft.Json.JsonConvert.SerializeObject(sr);
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void stop()
		{
			stream.Stop();
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
			Console.Write("\t(" + imageQueue.Count+"-r"+this.stream.BytesReceived+") ");


			string base64ConvertedString = ImageToBase64(eventArgs.Frame, System.Drawing.Imaging.ImageFormat.Jpeg);

			if (imageQueue.Count > MAX_IMAGES_IN_QUEUE)
			{
				imageQueue.Dequeue(); //remove 1st/oldest frame from the queue
			}
            fpsCount++;
            if (watch.Elapsed.TotalMilliseconds >= 1000)
            {
                this.streamFPS = Math.Round(fpsCount/(watch.Elapsed.TotalMilliseconds/1000),2);
                fpsCount = 0;
                watch.Restart();                
            }
            imageQueue.Enqueue(base64ConvertedString);
			//Console.Write("\t("+eventArgs.Frame.Width + "x" + eventArgs.Frame.Height + "x" + eventArgs.Frame.PixelFormat.ToString()+")"+imageQueue.Count+"\t ");
			//Console.WriteLine();
		}

		/// <summary>
		/// Images to base64.
		/// </summary>
		/// <returns>The to base64.</returns>
		/// <param name="image">Image.</param>
		/// <param name="format">Format.</param>
		public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				ImageCodecInfo jpgEncoder = GetEncoder(format);

				// Create an Encoder object based on the GUID
				// for the Quality parameter category.
				System.Drawing.Imaging.Encoder myEncoder =
					System.Drawing.Imaging.Encoder.Quality;

				// Create an EncoderParameters object.
				// An EncoderParameters object has an array of EncoderParameter
				// objects. In this case, there is only one
				// EncoderParameter object in the array.
				EncoderParameters myEncoderParameters = new EncoderParameters(1);

				EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
				myEncoderParameters.Param[0] = myEncoderParameter;
				/******************************************************************************************************/

				// Convert Image to byte[]
				image.Save(ms, jpgEncoder,myEncoderParameters);
				byte[] imageBytes = ms.ToArray();

				// Convert byte[] to Base64 String
				string base64String = Convert.ToBase64String(imageBytes);
				return base64String;
			}
		}

		/// <summary>
		/// Gets the encoder.
		/// </summary>
		/// <returns>The encoder.</returns>
		/// <param name="format">Format.</param>
		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}



	}
}
