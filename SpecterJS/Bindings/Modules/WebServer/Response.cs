using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;

namespace SpecterJS.Bindings.Modules.WebServer
{
	public class Response : PropertyBag
	{
		private HttpListenerResponse response;
		private bool headersSent;

		public Response(HttpListenerResponse response)
		{
			this.response = response;
		}

		[ScriptMember(Name = "headers")]
		public dynamic Headers
		{
			get
			{
				var headers = new Microsoft.ClearScript.PropertyBag();
				foreach (var key in response.Headers.AllKeys)
				{
					headers.Add(key, response.Headers[key]);
				}
				return headers;
			}
			set
			{
				var headerNames = value.GetDynamicMemberNames();
				response.Headers.Clear();
				foreach (var key in headerNames)
				{
					response.Headers.Add(key, value[key]);
				}

			}
		}

		[ScriptMember(Name = "statusCode")]
		public int StatusCode { get; set; }

		[ScriptMember(Name = "setHeader")]
		public void SetHeader(string name, dynamic value)
		{
			if (name.Equals("Content-Length"))
			{
				//response.ContentLength64 = value;
			}
			else
				response.Headers.Set(name, value.ToString());
		}

		[ScriptMember(Name = "header")]
		public string GetHeader(string name)
		{
			return response.Headers.Get(name);
		}

		[ScriptMember(Name = "setEncoding")]
		public void SetEncoding(string encoding)
		{
			if (encoding.Equals("binary", StringComparison.OrdinalIgnoreCase))
			{
				response.ContentEncoding = Encoding.Default;
				return;
			}

			response.ContentEncoding = Encoding.GetEncodings()
				.Where(x => x.Name.Equals(encoding, StringComparison.OrdinalIgnoreCase))
				.Single().GetEncoding();
		}

		[ScriptMember(Name = "write")]
		public void Write(string data)
		{
			var encoding = response.ContentEncoding ?? Encoding.Default;
			var buffer = encoding.GetBytes(data);
			response.OutputStream.Write(buffer, 0, buffer.Length);
			response.OutputStream.Flush();
		}

		[ScriptMember(Name = "writeHead")]
		public void WriteHead(int statusCode, dynamic headers)
		{
			response.StatusCode = statusCode;
			Headers = headers;
			headersSent = true;
		}

		[ScriptMember(Name = "close")]
		public void Close()
		{
			response?.Close();
		}

		[ScriptMember(Name = "closeGracefully")]
		public void CloseGracefully()
		{
			if (!headersSent) WriteHead(200, null);
			Close();
		}
	}
}
