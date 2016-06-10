using Microsoft.ClearScript;
using mshtml;
using SpecterJS.Browser;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Dynamic;
using System.Collections.Generic;
using SpecterJS.Util;
using System.Threading;
using System.Threading.Tasks;

namespace SpecterJS.Bindings.Modules.WebPage
{
	public class WebPage : PropertyBag, IDisposable
	{
		private Browser.WebBrowser browser;
		private Rectangle? clipRect;
		private HtmlWindow frame;
		private string navigationType;
		private ScriptEngine engine;

		public WebPage(string libraryPath, bool ignoreSslErrors, ScriptEngine engine)
		{
			LibraryPath = libraryPath;
			this.engine = engine;
			Settings = new Settings();
			browser = new Browser.WebBrowser();
			browser.Size = new Size(400, 300);
			browser.ScrollBarsEnabled = false;
			browser.IgnoreSslErrors = ignoreSslErrors;
			browser.ObjectForScripting = new Bridge(this);

			browser.Navigated += delegate(object sender, WebBrowserNavigatedEventArgs e)
			{
				if (OnUrlChanged != null)
					ObjectHelpers.DynamicInvoke(OnUrlChanged, e.Url.AbsoluteUri);

				if (OnInitialized != null)
					ObjectHelpers.DynamicInvoke(OnInitialized);
			};
			
			browser.Closing += delegate (object sender, EventArgs e)
			{
				if (OnClosing != null)
					ObjectHelpers.DynamicInvoke(OnClosing);
			};

			navigationType = "Undefined";

			browser.BeforeNavigate2 += delegate (object sender, WebBrowserBeforeNavigate2EventArgs e)
			{
				var type = navigationType;
				switch ((int)e.Flags)
				{
					case 64:
						type = "LinkClicked";
						break;
					case 256:
						type = "Other";
						break;
					case 320:
						if (type != "Reload")
							type = "BackOrForward";
						break;
					default:
						if (type != null)
							type = "FormSubmitted";
						break;
				}

				if (OnNavigationRequested != null)
				{
					ObjectHelpers.DynamicInvoke(OnNavigationRequested, e.Url, type, browser.AllowNavigation, (e.Disp == browser.ActiveXInstance));
				}

				if (OnLoadStarted != null)
				{
					ObjectHelpers.DynamicInvoke(OnLoadStarted);
				}
			};
		}

		#region Properties
		[ScriptMember(Name = "canGoBack")]
		public bool CanGoBack
		{
			get
			{
				return browser.CanGoBack;
			}
		}

		[ScriptMember(Name = "canGoForward")]
		public bool CanGoForward
		{
			get
			{
				return browser.CanGoForward;
			}
		}

		[ScriptMember(Name = "clipRect")]
		public dynamic ClipRect
		{
			get
			{
				dynamic obj = new ExpandoObject();
				if (clipRect.HasValue)
				{
					obj.top = clipRect.Value.Y;
					obj.left = clipRect.Value.X;
					obj.width = clipRect.Value.Width;
					obj.height = clipRect.Value.Height;
				}
				else
				{
					obj.top = 0;
					obj.left = 0;
					obj.width = browser.Width;
					obj.height = browser.Height;
				}
				return obj;
			}
			set
			{
				clipRect = new Rectangle(value.left, value.top, value.width, value.height);
			}
		}

		[ScriptMember(Name = "content")]
		public string Content
		{
			get
			{
				return browser.DocumentText;
			}
		}

		[ScriptMember(Name = "cookies")]
		public object[] Cookies
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[ScriptMember(Name = "customHeaders")]
		public dynamic CustomHeaders { get; set; }

		[ScriptMember(Name = "event")]
		public Event Event
		{
			get
			{
				return new Event();
			}
		}

		[ScriptMember(Name = "focusedFrameName")]
		public string FocusedFrameName
		{
			get
			{
				var focusedFrame = browser.Document?.Window?.Frames
					.Cast<HtmlWindow>()
					.Where(x => x.Document.Focused.Equals(true))
					.SingleOrDefault();

				return focusedFrame?.Name ?? string.Empty;
			}
		}

		[ScriptMember(Name = "frameContent")]
		public string FrameContent
		{
			get
			{
				if (frame != null && frame.Document != null)
				{
					var doc = frame.Document.DomDocument as HTMLDocument;
					return doc?.documentElement?.outerHTML ?? string.Empty;
				}
				return string.Empty;
			}
		}

		[ScriptMember(Name = "frameName")]
		public string FrameName
		{
			get
			{
				return frame?.Name ?? string.Empty;
			}
		}

		[ScriptMember(Name = "framePlainText")]
		public string FramePlainText
		{
			get
			{
				return frame?.Document?.Body?.InnerText ?? string.Empty;
			}
		}

		[ScriptMember(Name = "frameTitle")]
		public string FrameTitle
		{
			get
			{
				return frame?.Document?.Title ?? string.Empty;
			}
		}

		[ScriptMember(Name = "frameUrl")]
		public string FrameUrl
		{
			get
			{
				return frame?.Url.AbsoluteUri ?? string.Empty;
			}
		}

		[ScriptMember(Name = "framesCount")]
		public int FramesCount
		{
			get
			{
				return frame?.Frames?.Count ?? 0;
			}
		}

		[ScriptMember(Name = "framesName")]
		public IList<string> FramesName
		{
			get
			{
				var result = new List<string>();
				if (frame != null && frame.Frames != null)
				{
					foreach (HtmlWindow window in frame.Frames)
					{
						result.Add(window.Name);
					}
				}
				return result;
			}
		}

		[ScriptMember(Name = "libraryPath")]
		public string LibraryPath { get; set; }

		[ScriptMember(Name = "navigationLocked")]
		public bool NavigationLocked
		{
			get
			{
				return !browser.AllowNavigation;
			}
			set
			{
				browser.AllowNavigation = !value;
			}
		}

		[ScriptMember(Name = "offlineStoragePath")]
		public string OfflineStoragePath
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[ScriptMember(Name = "offlineStorageQuota")]
		public int OfflineStorageQuota
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[ScriptMember(Name = "ownsPages")]
		public bool OwnsPages { get; set; }

		[ScriptMember(Name = "pages")]
		public object Pages
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[ScriptMember(Name = "pagesWindowName")]
		public string PagesWindowName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[ScriptMember(Name = "paperSize")]
		public object PaperSize
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[ScriptMember(Name = "plainText")]
		public string PlainText
		{
			get
			{
				return browser.Document?.Body?.InnerText ?? string.Empty;
			}
		}
		
		[ScriptMember(Name = "scrollPosition")]
		public dynamic ScrollPosition
		{
			get
			{
				dynamic scroll = new ExpandoObject();
				scroll.top = browser.Document?.Window?.Position.Y ?? 0;
				scroll.left = browser.Document?.Window?.Position.X ?? 0;
				return scroll;
            }
			set
			{
				browser.Document.Window.ScrollTo(value.left, value.top);
			}
		}

		private Settings _settings;
		[ScriptMember(Name = "settings")]
		public dynamic Settings {
			set
			{
				_settings = value;
			}
			get
			{
				return _settings;
			}
		}

		[ScriptMember(Name = "title")]
		public string Title
		{
			get
			{
				return browser.DocumentTitle;
			}
		}

		[ScriptMember(Name = "url")]
		public string Url
		{
			get
			{
				return browser.Url.AbsoluteUri;
			}
		}

		[ScriptMember(Name = "viewportSize")]
		public dynamic ViewportSize
		{
			get
			{
				dynamic obj = new ExpandoObject();
				obj.width = browser.Width;
				obj.height = browser.Height;
				return obj;
			}
			set
			{
				browser.Size = new Size(value.width, value.height);
			}
		}

		[ScriptMember(Name = "windowName")]
		public string WindowName
		{
			get
			{
				return browser.Document?.Window?.Name ?? string.Empty;
			}
		}

		[ScriptMember(Name = "zoomFactor")]
		public double ZoomFactor { get; set; }
		#endregion

		#region Methods
		[ScriptMember(Name = "addCookie")]
		public void AddCookie(object cookie)
		{
			throw new NotImplementedException();
		}

		[ScriptMember(Name = "clearCookies")]
		public void ClearCookies()
		{
			throw new NotImplementedException();
		}

		[ScriptMember(Name = "close")]
		public void Close()
		{
			if (!browser.IsDisposed)
				browser.Dispose();
		}

		[ScriptMember(Name = "deleteCookie")]
		public void DeleteCookie(string name)
		{
			throw new NotImplementedException();
		}

		[ScriptMember(Name = "evaluate")]
		public object Evaluate(dynamic func, params dynamic[] args)
		{
			var source = func is string ? func : func.toString();
			var parameters = new List<string>();
			foreach (var arg in args)
			{
				if (arg is Undefined)
					parameters.Add("null");
				else
					parameters.Add(engine.Script.JSON.stringify(arg));
			}
			source = string.Format("JSON.stringify(({0})({1}));", source, string.Join(",", parameters));
			return engine.Script.JSON.parse(Eval(source));
		}

		[ScriptMember(Name = "evaluateAsync")]
		public void EvaluateAsync(dynamic func, int delayMillis = 0, params dynamic[] args)
		{
			Task.Factory.StartNew(() => {
				Thread.Sleep(delayMillis);
				Evaluate(func, args);
			});
		}

		[ScriptMember(Name = "evaluateJavascript")]
		public object EvaluateJavascript(string source)
		{
			source = string.Format("({0})();", source);
			return Eval(source);
		}

		private object Eval(string source)
		{
			var eval = new Func<string, object>((string src) =>
			{
				var doc = frame?.Document ?? browser.Document;
				return doc.InvokeScript("eval", new object[] { src });
			});
			return browser.Invoke(eval, source);
		}

		[ScriptMember(Name = "getPage")]
		public object GetPage(string windowName)
		{
			throw new NotImplementedException();
		}

		[ScriptMember(Name = "go")]
		public void Go(int index)
		{
			if (index != 0 && browser.Document?.Window?.History != null)
			{
				navigationType = "BackOrForward";
				browser.Document.Window.History.Go(index);
			}
		}

		[ScriptMember(Name = "goBack")]
		public void GoBack()
		{
			if (CanGoBack)
			{
				navigationType = "BackOrForward";
				browser.GoBack();
			}
		}

		[ScriptMember(Name = "goForward")]
		public void GoForward()
		{
			if (CanGoBack)
			{
				navigationType = "BackOrForward";
				browser.GoForward();
			}
		}

		[ScriptMember(Name = "includeJs")]
		public void IncludeJs(string url, dynamic callback)
		{
			var element = browser.Document.CreateElement("script");
			var script = element.DomElement as IHTMLScriptElement;
			script.src = url;
			script.type = "text/javascript";
			var doc = frame?.Document ?? browser.Document;
			doc.Body.AppendChild(element);
			var eventTarget = (HTMLScriptEvents2_Event)script;
			HTMLScriptEvents2_onreadystatechangeEventHandler listener = null;

			listener = delegate
			{
				if (script.readyState == "complete" || script.readyState == "loaded")
				{
					eventTarget.onreadystatechange -= listener;
					callback();
				}
			};
			eventTarget.onreadystatechange += listener;
		}

		[ScriptMember(Name = "injectJs")]
		public bool InjectJs(string filename)
		{
			if (!File.Exists(filename) && !File.Exists(Path.Combine(LibraryPath, filename)))
				return false;

			if (!File.Exists(filename)) filename = Path.Combine(LibraryPath, filename);

			var element = browser.Document.CreateElement("script");
			var script = element.DomElement as IHTMLScriptElement;
			script.text = File.ReadAllText(filename);
			script.type = "text/javascript";
			var doc = frame?.Document ?? browser.Document;
			doc.Body.AppendChild(element);

			return true;
		}

		[ScriptMember(Name = "open")]
		public void Open(string url, dynamic callback = null)
		{
			this.Open(url, "GET", callback);
		}

		[ScriptMember(Name = "open")]
		public void Open(string url, string method, dynamic callback = null)
		{
			this.Open(url, method, string.Empty, callback);
		}

		[ScriptMember(Name = "open")]
		public void Open(string url, string method, string data, dynamic callback = null)
		{
			WebBrowserNavigateErrorEventHandler fail = null;
			WebBrowserDocumentCompletedEventHandler completed = null;
			WebBrowserNavigatedEventHandler navigated = null;

			navigationType = "Other";
			var frameCount = 0;
			var done = false;

			navigated = delegate (object sender, WebBrowserNavigatedEventArgs e)
			{
				browser.Document.Window.Error += delegate (object sender2, HtmlElementErrorEventArgs e2)
				{
					e2.Handled = true;

					if (OnError != null)
					{
						dynamic err = new ExpandoObject();
						err.file = e2.Url.AbsoluteUri;
						err.line = e2.LineNumber;

						ObjectHelpers.DynamicInvoke(OnError, e2.Description, new dynamic[] { err });
					}
				};
			};

			fail = new WebBrowserNavigateErrorEventHandler(delegate (object s, WebBrowserNavigateErrorEventArgs e)
			{
				browser.NavigateError -= fail;
				browser.DocumentCompleted -= completed;
				browser.Navigated -= navigated;

				if (callback != null)
					ObjectHelpers.DynamicInvoke(callback, "fail");

				done = true;

				if (OnLoadFinished != null)
				{
					ObjectHelpers.DynamicInvoke(OnLoadFinished, "fail");
				}
				navigationType = "Unknown";
			});

           

			completed = new WebBrowserDocumentCompletedEventHandler(delegate (object s, WebBrowserDocumentCompletedEventArgs e)
			{
				frameCount++;

				var complete = false;

				if (browser.Document != null)
				{
					HtmlWindow win = browser.Document.Window;
					if (!(win.Frames.Count > frameCount && win.Frames.Count > 0))
						complete = true;
				}
				else complete = true;


				if (complete)
				{
					EvaluateAsync(ResourceHelpers.ReadResource("browser/bootstrap.js"));
					browser.NavigateError -= fail;
					browser.DocumentCompleted -= completed;
					frame = browser.Document.Window;
					browser.Navigated -= navigated;

					var width = browser.Document.Body.ScrollRectangle.Width;
					var height = browser.Document.Body.ScrollRectangle.Height;
					browser.Size = new Size(width, height);

					if (callback != null)
						ObjectHelpers.DynamicInvoke(callback, "success");

					done = true;

					if (OnLoadFinished != null)
					{
						ObjectHelpers.DynamicInvoke(OnLoadFinished, "success");
					}
					navigationType = "Unknown";
				}
			});

			var headers = string.Empty;

			if (CustomHeaders != null)
			{
				foreach (var name in CustomHeaders.GetDynamicMemberNames())
				{
					headers += string.Format("{0}: {1}\r\n", (string)name, CustomHeaders[name]);
				}
			}

			switch (method)
			{
				case "GET":
					browser.NavigateError += fail;
					browser.DocumentCompleted += completed;
					browser.Navigated += navigated;
                    browser.Navigate(url, "", null, headers);
					break;
				case "POST":
					if (string.IsNullOrEmpty(data)) data = " ";
					browser.NavigateError += fail;
					browser.DocumentCompleted += completed;
					browser.Navigated += navigated;
					browser.Navigate(url, "", global::System.Text.Encoding.UTF8.GetBytes(data), headers);
					break;
				default:
					throw new Exception();
			}

			while (callback == null && !done)
			{
				Thread.Sleep(10);
				Application.DoEvents();
			}
		}

		[ScriptMember(Name = "open")]
		public void Open(string url, string method, object settings, object callback)
		{
			throw new NotImplementedException();
		}

		[ScriptMember(Name = "openUrl")]
		public void OpenUrl(string url, object httpConf, object settings)
		{
			throw new NotImplementedException();
		}

		[ScriptMember(Name = "reload")]
		public void Reload()
		{
			navigationType = "Reload";
			browser.Refresh();
		}

		[ScriptMember(Name = "render")]
		public void Render(string filename, dynamic settings = null)
		{
			string format = settings?.format ?? Path.GetExtension(filename).Replace(".", "");
			int quality = settings?.quality ?? 100;

			ImageFormat imgFormat = null;
			switch (format.ToLower())
			{
				case "bmp":
					imgFormat = ImageFormat.Bmp;
					break;
				case "gif":
					imgFormat = ImageFormat.Gif;
					break;
				case "jpeg":
				case "jpg":
					imgFormat = ImageFormat.Jpeg;
					break;
				case "pdf":
					throw new NotSupportedException();
				case "png":
					imgFormat = ImageFormat.Png;
					break;
				case "ppm":
					throw new NotSupportedException();
				default:
					throw new NotSupportedException();
			}

			var rect = clipRect.HasValue ? clipRect.Value : new Rectangle(0, 0, browser.Width, browser.Height);
			using (Bitmap output = new Bitmap(rect.Width, rect.Height))
			{
				var encoder = ImageCodecInfo.GetImageEncoders().Where(x => x.FormatID == imgFormat.Guid).Single();
				var encoderParams = new EncoderParameters(1);
				encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
				browser.DrawToBitmap(output, rect);
				output.Save(filename, encoder, encoderParams);
			}
		}

		[ScriptMember(Name = "renderBase64")]
		public string RenderBase64(string format)
		{
			ImageFormat imgFormat = null;
			switch (format.ToLower())
			{
				case "gif":
					imgFormat = ImageFormat.Gif;
					break;
				case "jpeg":
					imgFormat = ImageFormat.Jpeg;
					break;
				case "png":
					imgFormat = ImageFormat.Png;
					break;
				default:
					throw new NotSupportedException();
			}

			var rect = clipRect.HasValue ? clipRect.Value : new Rectangle(0, 0, browser.Width, browser.Height);
			using (Bitmap output = new Bitmap(rect.Width, rect.Height))
			{
				var stream = new MemoryStream();

				browser.Invoke(new Action(() => browser.DrawToBitmap(output, rect)));
				output.Save(stream, imgFormat);
				return Convert.ToBase64String(stream.ToArray());
			}
		}

		[ScriptMember(Name = "sendEvent")]
		public void SendEvent(string eventName, params object[] args)
		{
			string key;
			switch (eventName)
			{
				case "mouseup":
				case "mousedown":
				case "mousemove":
				case "doubleclick":
				case "click":
					//if (args.Length > 0) evt.clientX = (int)args[0];
					//if (args.Length > 1) evt.clientY = (int)args[1];
					//if (args.Length > 2)
					//	evt.button = (string)args[2] == "left" ? 1
					//		: (string)args[2] == "right" ? 2
					//		: 4;
					//else evt.button = 1;
					break;
				case "keyup":

					key = args[0] as string;
					switch (key)
					{
						case "Shift":
							browser.KeyUp((int)Keys.Shift);
							break;
						case "Ctrl":
							browser.KeyUp((int)Keys.Control);
							break;
						case "Alt":
							browser.KeyUp((int)Keys.Alt);
							break;
					}
					break;

				case "keydown":
					key = args[0] as string;
					
					switch (key)
					{
						case "Shift":
							browser.KeyDown((int)Keys.Shift);
							break;
						case "Ctrl":
							browser.KeyDown((int)Keys.Control);
							break;
						case "Alt":
							browser.KeyDown((int)Keys.Alt);
							break;

					}
					break;
				case "keypress":
					key = args[0] as string;
					var keyCode = (int)key[0];

					browser.KeyPress(keyCode);
					break;
			}
		}

		[ScriptMember(Name = "setContent")]
		public void SetContent(string expectedContent, string expectedUrl)
		{
			browser.Url = new Uri(expectedUrl);
			browser.DocumentText = expectedContent;
		}

		[ScriptMember(Name = "stop")]
		public void Stop()
		{
			browser.Stop();
		}

		[ScriptMember(Name = "switchToFocusedFrame")]
		public void SwitchToFocusedFrame()
		{
			var focusedFrame = frame?.Frames.Cast<HtmlWindow>()
				.Where(x => x.Document.Focused.Equals(true))
				.SingleOrDefault();

			if (focusedFrame != null)
				frame = focusedFrame;
		}

		[ScriptMember(Name = "switchToFrame")]
		public bool SwitchToFrame(string name)
		{
			var newFrame = frame?.Frames.Cast<HtmlWindow>().Where(x => x.Name == name).SingleOrDefault();
			if (newFrame != null)
			{
				frame = newFrame;
				return true;
			}
			return false;
		}

		[ScriptMember(Name = "switchToFrame")]
		public bool SwitchToFrame(int position)
		{
			if (frame == null || frame.Frames.Count < position)
				return false;

			frame = frame.Frames[position];
			return true;
		}

		[ScriptMember(Name = "switchToMainFrame")]
		public void SwitchToMainFrame()
		{
			frame = browser.Document?.Window;
		}

		[ScriptMember(Name = "switchToParentFrame")]
		public void SwitchToParentFrame()
		{
			if (frame != null && frame.Parent != null)
				frame = frame.Parent;
		}

		[ScriptMember(Name = "uploadFile")]
		public void UploadFile(string selector, string filename)
		{
			throw new NotImplementedException();
		}

		[NoScriptAccess]
		public void Dispose()
		{
			if (!browser.IsDisposed)
				browser.Dispose();
		}
		#endregion

		#region Callbacks
		[ScriptMember(Name = "onAlert")]
		public DynamicObject OnAlert { get; set; }

		[ScriptMember(Name = "onCallback")]
		public DynamicObject OnCallback { get; set; }

		[ScriptMember(Name = "onClosing")]
		public DynamicObject OnClosing { get; set; }

		[ScriptMember(Name = "onConfirm")]
		public DynamicObject OnConfirm { get; set; }

		[ScriptMember(Name = "onConsoleMessage")]
		public DynamicObject OnConsoleMessage { get; set; }

		[ScriptMember(Name = "onError")]
		public DynamicObject OnError { get; set; }

		[ScriptMember(Name = "onFilePicker")]
		public DynamicObject OnFilePicker { get; set; }

		[ScriptMember(Name = "onInitialized")]
		public DynamicObject OnInitialized { get; set; }

		[ScriptMember(Name = "onLoadFinished")]
		public DynamicObject OnLoadFinished { get; set; }

		[ScriptMember(Name = "onLoadStarted")]
		public DynamicObject OnLoadStarted { get; set; }

		[ScriptMember(Name = "onNavigationRequested")]
		public DynamicObject OnNavigationRequested { get; set; }

		[ScriptMember(Name = "onPageCreated")]
		public DynamicObject OnPageCreated { get; set; }

		[ScriptMember(Name = "onPrompt")]
		public DynamicObject OnPrompt { get; set; }

		[ScriptMember(Name = "onResourceError")]
		public DynamicObject OnResourceError { get; set; }

		[ScriptMember(Name = "onResourceReceived")]
		public DynamicObject OnResourceReceived { get; set; }

		[ScriptMember(Name = "onResourceRequested")]
		public DynamicObject OnResourceRequested { get; set; }

		[ScriptMember(Name = "onResourceTimeout")]
		public DynamicObject OnResourceTimeout { get; set; }

		[ScriptMember(Name = "onUrlChanged")]
		public DynamicObject OnUrlChanged { get; set; }
		#endregion
	}
}
