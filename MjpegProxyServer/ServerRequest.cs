using System.Collections.Generic;
using Newtonsoft.Json;

namespace MjpegProxyServer
{
	public class ServerRequest
	{
		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		/// <value>The command.</value>
		[JsonProperty("command")]
		public string command { get; set; }
		/// <summary>
		/// Gets or sets the ip address.
		/// </summary>
		/// <value>The ip address.</value>
		[JsonProperty("ipAddress")]
		public string ipAddress { get; set; }
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>The unique identifier.</value>
		[JsonProperty("uniqueId")]
		public string uniqueId { get; set; }
		/// <summary>
		/// The is done.
		/// </summary>
		[JsonProperty("isDone")]
		public bool isDone;
		/// <summary>
		/// The client data.
		/// </summary>
		[JsonProperty("clientData")]
		public Dictionary<string, object> clientData;
		/// <summary>
		/// The server data.
		/// </summary>
		[JsonProperty("serverData")]
		public Dictionary<string, object> serverData;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MjpegProxyServer.ServerRequest"/> class.
		/// </summary>
		public ServerRequest()
		{
			clientData = new Dictionary<string, object>();
			serverData = new Dictionary<string, object>();
		}
	}
}


