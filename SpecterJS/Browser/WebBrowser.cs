using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Input;

namespace SpecterJS.Browser
{
	public class WebBrowser : System.Windows.Forms.WebBrowser
	{
		public static Guid IID_IHttpSecurity = new Guid("79eac9d7-bafa-11ce-8c82-00aa004ba90b");
		public static Guid IID_IWindowForBindingUI = new Guid("79eac9d5-bafa-11ce-8c82-00aa004ba90b");
		private const int WM_PARENTNOTIFY = 0x210;
		private const int WM_DESTROY = 2;

		AxHost.ConnectionPointCookie cookie;
		WebBrowser2EventHelper helper;


		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(IntPtr hwnd, uint msg, int wparam, IntPtr lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);


		public const int WM_KEYDOWN = 0x0100;
		public const int WM_KEYUP = 0x0101;
		public const int WM_CHAR = 0x102;


		private IntPtr GetHandle()
		{
			IntPtr pControl;
			IntPtr pControl2;
			pControl = FindWindowEx(this.Handle, IntPtr.Zero, "Shell Embedding", null);
			pControl = FindWindowEx(pControl, IntPtr.Zero, "Shell DocObject View", null);
			pControl = FindWindowEx(pControl, IntPtr.Zero, "Internet Explorer_Server", null);
			pControl2 = FindWindowEx(pControl, IntPtr.Zero, "MacromediaFlashPlayerActiveX", null);

			if (pControl2 != IntPtr.Zero)
				pControl = pControl2;
			return pControl;
		}

		public new void KeyDown(int key)
		{
			PostMessage(GetHandle(), WM_KEYDOWN, key, IntPtr.Zero);
		}

		public new void KeyUp(int key)
		{
			PostMessage(GetHandle(), WM_KEYUP, key, IntPtr.Zero);
		}

		public new void KeyPress(int key)
		{
			PostMessage(GetHandle(), WM_CHAR, key, IntPtr.Zero);
		}

		public bool IgnoreSslErrors { get; set; }

		public event ClosingEventHandler Closing;

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_PARENTNOTIFY:
					if (!DesignMode)
					{
						if (m.WParam.ToInt32() == WM_DESTROY)
						{
							Closing(this, EventArgs.Empty);
						}
					}
					DefWndProc(ref m);
					break;
				default:
					base.WndProc(ref m);
					break;
			}
		}

		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		protected override void CreateSink()
		{
			base.CreateSink();

			helper = new WebBrowser2EventHelper(this);
			cookie = new AxHost.ConnectionPointCookie(
				this.ActiveXInstance, helper, typeof(DWebBrowserEvents2));
		}

		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		protected override void DetachSink()
		{
			if (cookie != null)
			{
				cookie.Disconnect();
				cookie = null;
			}
			base.DetachSink();
		}

		public event WebBrowserNavigateErrorEventHandler NavigateError;
		public event WebBrowserNavigateCompleteEventHandler NavigateComplete;
		public event WebBrowserNewWindow3EventHandler NewWindow3;
		public event WebBrowserOnQuitEventHandler OnQuit;
		public event WebBrowserBeforeNavigate2EventHandler BeforeNavigate2;

		protected virtual void OnNavigateError(
			WebBrowserNavigateErrorEventArgs e)
		{
			if (this.NavigateError != null)
			{
				this.NavigateError(this, e);
			}
		}

		protected virtual void OnNavigateComplete(
			WebBrowserNavigateCompleteEventArgs e)
		{
			if (this.NavigateComplete != null)
			{
				this.NavigateComplete(this, e);
			}
		}

		protected virtual void OnNewWindow3(
			WebBrowserNewWindow3EventArgs e)
		{
			if (this.NewWindow3 != null)
			{
				this.NewWindow3(this, e);
			}
		}


		protected virtual void OnOnQuit(
			WebBrowserOnQuitEventArgs e)
		{
			if (this.OnQuit != null)
			{
				this.OnQuit(this, e);
			}
		}

		protected virtual void OnBeforeNavigate2(
			WebBrowserBeforeNavigate2EventArgs e)
		{
			if (this.BeforeNavigate2 != null)
			{
				this.BeforeNavigate2(this, e);
			}
		}

		private class WebBrowser2EventHelper :
			StandardOleMarshalObject, DWebBrowserEvents2
		{
			private WebBrowser parent;

			public WebBrowser2EventHelper(WebBrowser parent)
			{
				this.parent = parent;
			}

			public void NavigateError(object pDisp, ref object url,
				ref object frame, ref object statusCode, ref bool cancel)
			{
				this.parent.OnNavigateError(
					new WebBrowserNavigateErrorEventArgs(
					(String)url, (String)frame, (Int32)statusCode, cancel));
			}

			public void NavigateComplete2(object pDisp, ref object url)
			{
				this.parent.OnNavigateComplete(
					new WebBrowserNavigateCompleteEventArgs(
						(String)url));
			}

			public void NewWindow3([In, MarshalAs(UnmanagedType.IDispatch), Out] ref object ppDisp, [In, Out] ref bool Cancel, [In] uint dwFlags, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrlContext, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl)
			{
				this.parent.OnNewWindow3(
					new WebBrowserNewWindow3EventArgs(
						Cancel, dwFlags, bstrUrlContext, bstrUrl));
			}

			public void OnQuit()
			{
				this.parent.OnOnQuit(
					new WebBrowserOnQuitEventArgs());
			}

			public void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In, Out] ref bool cancel)
			{
				this.parent.OnBeforeNavigate2(
					new WebBrowserBeforeNavigate2EventArgs(
						pDisp, (string)url, flags, (string)targetFrameName, postData, headers, cancel));
			}

		}

		protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
		{
			return base.CreateWebBrowserSiteBase();
		}

		protected class SslWebBrowserSite : WebBrowserSite, IHttpSecurity
		{
			public const int S_OK = 0;
			public const int S_FALSE = 1;

			private WebBrowser host;

			public SslWebBrowserSite(WebBrowser host)
				: base(host)
			{
				this.host = host;
			}

			[return: MarshalAs(UnmanagedType.I4)]
			public int GetWindow([In] ref Guid rguidReason, [In, Out] ref IntPtr phwnd)
			{

				if (rguidReason == IID_IHttpSecurity
					|| rguidReason == IID_IWindowForBindingUI)
				{
					phwnd = this.host.Handle;
					return S_OK;
				}
				else
				{
					phwnd = IntPtr.Zero;
					return S_FALSE;
				}
			}

			public int OnSecurityProblem([In, MarshalAs(UnmanagedType.U4)] uint dwProblem)
			{
				if (host.IgnoreSslErrors)
					return S_OK;
				return S_FALSE;
			}
		}
	}

	public delegate void ClosingEventHandler(object sender, EventArgs e);
}
