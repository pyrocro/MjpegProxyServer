using System;
using System.Threading;
using WebSocketSharp.Server;

namespace MjpegProxyServer
{
	class MainClass
	{
		
		static void Main(string[] args)
		{
			//MjpegStreamManager.Start();
			WebSocketSharpStart();
			//SuperWebsocketStart();

		}
		/*static void SuperWebsocketStart()
		{
			var appServer = new WebSocketServer();
			Console.WriteLine(appServer.Name);
			appServer.Setup(6021);
			appServer.NewMessageReceived  += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);


			appServer.Start();
		}

		static void appServer_NewSessionConnected(AppSession session)
		{
			//session.Send("Welcome to SuperSocket Telnet Server");
			Console.WriteLine("connected to SuperSocket");
		}

		static void appServer_NewMessageReceived(WebSocketSession session, string message)
		{
			Console.WriteLine(message);
		}*/


		static void WebSocketSharpStart()
		{
            WebSocketServer server = new WebSocketServer(6021);
            MjpegStreamManager streamManager = new MjpegStreamManager();
            var mjpeg_url = Environment.GetEnvironmentVariable("MJPEG_URL");
            if (string.IsNullOrEmpty(mjpeg_url)){
                mjpeg_url = "http://ymc.redirectme.com/turtlecam"
            } 
            streamManager.addStream("TurtleCam",mjpeg_url);// "http://ymc.noip.me/turtlecam");
            //var api = new MjpegWebSocketServer();
            server.AddWebSocketService<MjpegWebSocketBehavior>("/api", () => new MjpegWebSocketBehavior(streamManager));

			//server.AddWebSocketService<MjpegWebSocketBehavior>("/api",() => new MjpegWebSocketBehavior("http://ymc.noip.me/turtlecam"));//"http://ymc.noip.me:8081"));
			//server.AddWebSocketService<MjpegWebSocketServer>("/api", () => new MjpegWebSocketServer("http://206.176.34.55/mjpg/video.mjpg?COUNTER"));
			Console.WriteLine("mjpeg_url url: "+mjpeg_url);

			Console.WriteLine("Starting Web Socket server"); 
			server.Start();
			Console.WriteLine("Web Socket server Started...........");


            //Console.ReadLine();
            //Thread.Sleep(50000000);
            do
            {
                Console.WriteLine(""+DateTime.Now.ToShortTimeString()+"()is listening" + server.IsListening);
                Thread.Sleep(5000);
                /*  while (!Console.KeyAvailable) //Continue if pressing a Key press is not available in the input stream
                    {
                        //Do Something While Paused
                    } 
                */

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape); //Resume if Escape was pressed
            Console.WriteLine("closing server...");
			server.Stop();
		}


	}
}
