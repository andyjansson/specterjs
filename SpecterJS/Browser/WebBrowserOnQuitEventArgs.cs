using System;

namespace SpecterJS.Browser
{
	public class WebBrowserOnQuitEventArgs : EventArgs
	{
		public WebBrowserOnQuitEventArgs()
		{

		}
	}

	public delegate void WebBrowserOnQuitEventHandler(object sender,
		WebBrowserOnQuitEventArgs e);
}
