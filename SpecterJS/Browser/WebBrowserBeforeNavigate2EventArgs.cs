using System;

namespace SpecterJS.Browser
{
	public class WebBrowserBeforeNavigate2EventArgs : EventArgs
	{
		public WebBrowserBeforeNavigate2EventArgs(object pDisp, string url, object flags, string targetFrameName, object postData, object headers, bool cancel)
		{
			Disp = pDisp;
			Url = url;
			Flags = flags;
			TargetFrameName = targetFrameName;
			PostData = PostData;
			Headers = headers;
			Cancel = cancel;
		}

		public object Disp { get; set; }
		public string Url { get; set; }
		public object Flags { get; set; }
		public string TargetFrameName { get; set; }
		public object PostData { get; set; }
		public object Headers { get; set; }
		public bool Cancel { get; set; }
	}

	public delegate void WebBrowserBeforeNavigate2EventHandler(object sender, WebBrowserBeforeNavigate2EventArgs e);
}
