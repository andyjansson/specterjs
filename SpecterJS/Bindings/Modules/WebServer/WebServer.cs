using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SpecterJS.Bindings.Modules.WebServer
{
	public class WebServer : IDisposable
	{
		private IDictionary<Uri, Listener> listeners;
		
		public WebServer()
		{
			listeners = new Dictionary<Uri, Listener>();
		}

		[NoScriptAccess]
		public void Dispose()
		{
			foreach (var listener in listeners.Values)
			{
				listener.Close();
			}
			listeners.Clear();
		}

		[ScriptMember(Name = "listen")]
		public Listener Listen(int port, DynamicObject callback)
		{
			return this.Listen(port, null, callback);
		}

		[ScriptMember(Name = "listen")]
		public Listener Listen(string addr, DynamicObject callback)
		{
			return this.Listen(addr, null, callback);
		}

		[ScriptMember(Name = "listen")]
		public Listener Listen(int port, dynamic opts, DynamicObject callback)
		{
			return this.Listen(string.Format("localhost:{0}", port), opts, callback);
		}

		[ScriptMember(Name = "listen")]
		public Listener Listen(string addr, dynamic opts, DynamicObject callback)
		{
			var url = new Uri(string.Format("http://{0}/", addr));
			if (url.Host.Equals("127.0.0.1"))
			{
				var builder = new UriBuilder(url);
				builder.Host = "localhost";
				url = builder.Uri;
            }

			Listener listener;

			if (listeners.TryGetValue(url, out listener))
			{
				listener.Close();
				listeners.Remove(url);
			}

			listener = new Listener(url, opts, callback);
            listeners.Add(url, listener);
			return listener;
        }

		[ScriptMember(Name = "port")]
		public int Port
		{
			get
			{
				var listener = listeners.Values.Last();
				return listener?.Port ?? -1;
			}
		}
	}
}
