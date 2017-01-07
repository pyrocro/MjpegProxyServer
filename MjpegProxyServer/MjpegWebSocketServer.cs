using System;
using WebSocketSharp.Server;

namespace MjpegProxyServer
{
	public class WebSocketServer:WebSocketBehavior
	{
		public WebSocketServer()
		{
		}
		protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
		{
			base.OnMessage(e);
		}
	}
}
