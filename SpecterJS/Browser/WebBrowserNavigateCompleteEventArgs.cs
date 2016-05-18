using System;

namespace SpecterJS.Browser
{
	public class WebBrowserNavigateCompleteEventArgs : EventArgs
	{
		private String urlValue;

		public WebBrowserNavigateCompleteEventArgs(
			String url)
		{
			urlValue = url;
		}

		public String Url
		{
			get { return urlValue; }
			set { urlValue = value; }
		}
	}


	public delegate void WebBrowserNavigateCompleteEventHandler(object sender,
		WebBrowserNavigateCompleteEventArgs e);
}
