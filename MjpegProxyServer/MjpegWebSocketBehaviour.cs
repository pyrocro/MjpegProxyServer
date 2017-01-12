using System;
using Newtonsoft.Json;
using WebSocketSharp.Server;

namespace MjpegProxyServer
{
	public class MjpegWebSocketBehavior:WebSocketBehavior
	{
        MjpegStreamManager mjpegStreamManager = null;
        //public MjpegStream stream = null;

		/// <summary>
		/// Setups the mjpeg web socket server.
		/// </summary>
		public MjpegWebSocketBehavior():this(null)
		{
		}
		/*public MjpegWebSocketBehavior(string url)
		{           
			stream = new MjpegStream(url);
			stream.start();
		}*/
        public MjpegWebSocketBehavior(MjpegStreamManager sm)
        {
            mjpegStreamManager = sm;
        }
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
		{
			/*Console.WriteLine("**********");
			if (e.IsText) Console.WriteLine("IS TEXT"); else Console.WriteLine("IS Binary");
			Console.WriteLine("request = " + e.RawData.Length);
			Console.WriteLine("**********");*/
			var request = JsonConvert.DeserializeObject<ServerRequest>(e.Data);
            string str = null;
			switch (request.command)
			{
				case "ready":

                    MjpegStream stream = mjpegStreamManager.getMjpegStream(request.clientData["stream_name"].ToString(),this);
                    if (stream.isRunning() == false) stream.start(); // if the stream is not running start it
					request.uniqueId = this.ID;
					request.serverData.Add("image", "data:image/jpeg;base64,"+stream.currentImage);
                    request.serverData.Add("ServerStreamFPS", stream.streamFPS);
                    request.serverData.Add("ServerBufferedFrames", stream.QueueCount);

                    str = JsonConvert.SerializeObject(request);
                    this.Send(str);
                    //this.SendAsync(s,null);
                    break;
                case "get_mjpeg_stream":
                    
                    break;
				

			}
		}
		protected override void OnClose(WebSocketSharp.CloseEventArgs e)
		{
            mjpegStreamManager.removeConnectionReference(this.ID);			
			base.OnClose(e);
		}
		protected override void OnError(WebSocketSharp.ErrorEventArgs e)
		{
			Console.WriteLine("Websocket Error---" + e.Message);
			
			base.OnError(e);
		}
		protected override void OnOpen()
		{
			base.OnOpen();
			Console.WriteLine("***************************************CONECTED*****************************************");
		}

	}
}
