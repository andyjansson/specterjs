using Microsoft.ClearScript;
using System;

namespace SpecterJS.Bindings.Modules.WebServer
{
	public class WebServer : PropertyBag
	{
		private Listener listener;

		public WebServer()
		{
			NewRequest = new Connection();
		}

		[ScriptMember("newRequest")]
		public Connection NewRequest { get; private set; }

		[ScriptMember(Name = "listenOnPort")]
		public Listener ListenOnPort(string portOrAddr, dynamic opts)
		{
			var addr = string.Empty;
			int port = 0;

			if (int.TryParse(portOrAddr, out port))
				addr = string.Format("http://localhost:{0}/", port);
			else
				addr = string.Format("http://{0}/", portOrAddr);

			var url = new Uri(addr);
			if (url.Host.Equals("127.0.0.1"))
			{
				var builder = new UriBuilder(url);
				builder.Host = "localhost";
				url = builder.Uri;
			}

			listener = new Listener(url, opts, NewRequest);
			return listener;
		}

		[ScriptMember(Name = "port")]
		public int Port
		{
			get
			{
				return listener?.Port ?? -1;
			}
		}
	}
}
