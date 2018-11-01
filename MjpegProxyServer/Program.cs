using System;
using System.IO;
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
        static void readConfig(MjpegStreamManager msm)
        {
            if (msm == null) return;
            int counter = 0;
            string line = null;
            System.IO.StreamReader file = new System.IO.StreamReader(@"streams.conf");
            System.Console.WriteLine("Loading streams.conf......");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(' ');                
                msm.addStream(words[0], words[1]);
                System.Console.WriteLine(line);
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
        }

		static void WebSocketSharpStart()
		{
            
            WebSocketServer server = new WebSocketServer(6021);
            MjpegStreamManager streamManager = new MjpegStreamManager();
            var mjpeg_url = Environment.GetEnvironmentVariable("MJPEG_URL");
            Console.WriteLine("mjpeg_url url: " + mjpeg_url);
            //Load Config file 
            var confFileExist = File.Exists("streams.conf");
            Console.WriteLine(confFileExist ? "streams.conf File exists." : "streams.conf File does not exist.");
            if (confFileExist)
            {
                readConfig(streamManager);
            }
            
            if (string.IsNullOrEmpty(mjpeg_url))
            {
                mjpeg_url = "http://dd191c72.ngrok.io";
            }
            streamManager.addStream("TurtleCam", mjpeg_url);// "http://ymc.noip.me/turtlecam");
            /*streamManager.addStream("rp_1", "http://192.168.2.100:8081");
            streamManager.addStream("rp_2", "http://192.168.2.108:8081");
            streamManager.addStream("rp_3", "http://200.36.58.250/mjpg/video.mjpg?resolution=640x480");
            streamManager.addStream("rp_4", "http://200.36.58.250/mjpg/video.mjpg?resolution=640x480");
            streamManager.addStream("rp_5", "http://200.36.58.250/mjpg/video.mjpg?resolution=640x480");*/

            

            
            //var api = new MjpegWebSocketServer();
            server.AddWebSocketService<MjpegWebSocketBehavior>("/api", () => new MjpegWebSocketBehavior(streamManager));

			//server.AddWebSocketService<MjpegWebSocketBehavior>("/api",() => new MjpegWebSocketBehavior("http://ymc.noip.me/turtlecam"));//"http://ymc.noip.me:8081"));
			//server.AddWebSocketService<MjpegWebSocketServer>("/api", () => new MjpegWebSocketServer("http://206.176.34.55/mjpg/video.mjpg?COUNTER"));
			

			Console.WriteLine("Starting Web Socket server"); 
			server.Start();
			Console.WriteLine("Web Socket server Started...........");


            //Console.ReadLine();
            //Thread.Sleep(50000000);
            do
            {
                Console.WriteLine(""+DateTime.Now.ToShortTimeString()+"()is listening" + server.IsListening);
                Console.WriteLine("mjpeg_url url: "+mjpeg_url);
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
