using System;
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
            streamManager.addStream("TurtleCam", "http://ymc.noip.me/turtlecam");
            //var api = new MjpegWebSocketServer();
            server.AddWebSocketService<MjpegWebSocketBehavior>("/api", () => new MjpegWebSocketBehavior(streamManager));

			//server.AddWebSocketService<MjpegWebSocketBehavior>("/api",() => new MjpegWebSocketBehavior("http://ymc.noip.me/turtlecam"));//"http://ymc.noip.me:8081"));
			//server.AddWebSocketService<MjpegWebSocketServer>("/api", () => new MjpegWebSocketServer("http://206.176.34.55/mjpg/video.mjpg?COUNTER"));

			Console.WriteLine("Starting Web Socket server");
			server.Start();
			Console.WriteLine("Web Socket server Started...........");

			Console.ReadLine();
			Console.WriteLine("closing server");
			server.Stop();
		}


	}
}
