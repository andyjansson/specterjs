using System;

namespace SpecterJS.Browser
{
	public class WebBrowserNewWindow3EventArgs : EventArgs
	{
		public WebBrowserNewWindow3EventArgs(
			bool cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
		{
			Cancel = cancel;
			Flags = dwFlags;
			UrlContext = bstrUrlContext;
			Url = bstrUrl;
		}

		public bool Cancel { get; set; }
		public uint Flags { get; set; }
		public string UrlContext { get; set; }
		public string Url { get; set; }
	}

	public delegate void WebBrowserNewWindow3EventHandler(object sender,
		WebBrowserNewWindow3EventArgs e);
}
