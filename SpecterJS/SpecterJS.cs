using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using SpecterJS.Bindings;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpecterJS
{
	public class Specter : ApplicationContext
	{
		private ScriptEngine engine;

		public string Filename { get; private set; }
		public IEnumerable<string> Arguments { get; private set; }

		public Specter(Options options)
		{
			Options = options;
			engine = new V8ScriptEngine();
			engine.AddHostObject("phantom", new Phantom(this));
			engine.AddHostType("Timer", typeof(Bindings.Timer));
		}

		public Options Options { get; private set; }

		public Specter Execute(string code, string filename, IEnumerable<string> args)
		{
			Filename = filename;
			Arguments = args;

			if (OnExecute != null)
				OnExecute();

			engine.Execute(filename, code);
			return this;
		}

		public event ExecuteEvent OnExecute;
	}

	public delegate void ExecuteEvent();
}
