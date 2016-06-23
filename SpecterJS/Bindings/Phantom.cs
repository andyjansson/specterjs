using Microsoft.ClearScript;
using Newtonsoft.Json;
using SpecterJS.Bindings.Modules.ChildProcess;
using SpecterJS.Bindings.Modules.CookieJar;
using SpecterJS.Bindings.Modules.FileSystem;
using SpecterJS.Bindings.Modules.WebPage;
using SpecterJS.Bindings.Modules.WebServer;
using SpecterJS.Util;
using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace SpecterJS.Bindings
{
	public class Phantom : IScriptableObject
	{
		private ScriptEngine engine;
		private Specter specter;

		public Phantom(Specter specter)
		{
			this.specter = specter;
			specter.OnExecute += () =>
			{
				LibraryPath = !string.IsNullOrEmpty(specter.Filename)
					? Path.GetDirectoryName(Path.GetFullPath(specter.Filename))
					: string.Empty
				;
				engine.Execute(ResourceHelpers.ReadResource("bootstrap.js"));
			};
		}

		[ScriptMember(Name = "version")]
		public dynamic Version
		{
			get
			{
				dynamic version = new ExpandoObject();
				version.major = 0;
				version.minor = 1;
				version.patch = 0;
				return version;
			}
		}

		[ScriptMember(Name = "exit")]
		public void Exit(int returnValue = 0)
		{
			engine.Interrupt();
			Environment.ExitCode = returnValue;
			Task.Run(() => specter.ExitThread());
		}

		[ScriptMember(Name = "createSystem")]
		public Modules.System.System CreateSystem()
		{
			dynamic arguments = engine.Evaluate(JsonConvert.SerializeObject(specter.Arguments));
			return new Modules.System.System(arguments);
		}

		[ScriptMember(Name = "createFilesystem")]
		public FileSystem CreateFileSystem()
		{
			return new FileSystem();
		}

		[ScriptMember(Name = "createWebPage")]
		public WebPage CreateWebPage()
		{
			var webPage = new WebPage(LibraryPath, specter.Options.IgnoreSslErrors, engine);
			webPage.OnError = OnError;
			return webPage;
		}

		[ScriptMember(Name = "createWebServer")]
		public WebServer CreateWebServer()
		{
			return new WebServer();
		}

		[ScriptMember(Name = "createChildProcess")]
		public ChildProcess CreateChildProcess()
		{
			return new ChildProcess(this);
		}

		[ScriptMember(Name = "createCookieJar")]
		public CookieJar CreateCookieJar()
		{
			return new CookieJar();
		}

		[ScriptMember(Name = "libraryPath")]
		public string LibraryPath { get; set; }

		[ScriptMember(Name = "injectJs")]
		public bool InjectJs(string path)
		{
			var script = string.Empty;
			if (File.Exists(path))
			{
				script = File.ReadAllText(path);

			}
			else if (File.Exists(Path.Combine(LibraryPath, path)))
			{
				path = Path.Combine(LibraryPath, path);
				script = File.ReadAllText(path);
			}
			else
				script = ResourceHelpers.ReadResource(path);

			if (string.IsNullOrEmpty(script))
				return false;

			var oldPath = LibraryPath;
			LibraryPath = Path.GetDirectoryName(path);
			engine.Evaluate(path, script);
			LibraryPath = oldPath;

			return true;
		}

		[ScriptMember(Name = "loadModule")]
		public void LoadModule(string source, string filename)
		{
			var filenameEscaped = filename.Replace("\\", "\\\\");
			var script = "(function(require, exports, module) {\n" +
				source +
				"\n}.call({}," +
				"require.cache['" + filenameEscaped + "']._getRequire()," +
				"require.cache['" + filenameEscaped + "'].exports," +
				"require.cache['" + filenameEscaped + "']" +
				"));";

			engine.Evaluate(filename, script);
		}

		[ScriptMember(Name = "onError")]
		public dynamic OnError { get; set; }

		[NoScriptAccess]
		public void OnExposedToScriptCode(ScriptEngine engine)
		{
			this.engine = engine;
		}
	}
}
