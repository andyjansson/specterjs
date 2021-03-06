﻿using Microsoft.ClearScript;
using SpecterJS.Util;
using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SpecterJS.Bindings.Modules.WebServer
{
	public class Listener : IDisposable
	{
		private HttpListener listener;
		private Task acceptLoop;

		public Listener(Uri url, DynamicObject opts, Connection callback)
		{
			listener = new HttpListener();
			listener.Prefixes.Add(url.AbsoluteUri);
			listener.Start();
			Port = url.Port;
			Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
			var acceptConnection = new Action<HttpListenerContext>((HttpListenerContext context) =>
			{
				var request = new Request(context.Request);
				var response = new Response(context.Response);

				if (opts != null)
				{
					var keepAlive = ObjectHelpers.GetProperty(opts, "keepAlive");
					if (!(keepAlive is Undefined) && (bool)keepAlive == true)
					{
						context.Response.KeepAlive = true;
					}
				}
				callback.Write(request, response);
			});

			acceptLoop = Task.Run(async () =>
			{
				while (listener.IsListening)
				{
					var context = await listener.GetContextAsync();
					dispatcher.Invoke(acceptConnection, context);
				}
			});
		}

		[ScriptMember(Name = "close")]
		public void Close()
		{
			listener.Stop();
		}

		[NoScriptAccess]
		public void Dispose()
		{
			Close();
		}

		[NoScriptAccess]
		public int Port { get; private set; }
	}
}
