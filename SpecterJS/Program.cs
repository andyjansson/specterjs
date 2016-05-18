using System;
using SpecterJS.CommandLine;
using NDesk.Options;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.ClearScript;
using SpecterJS.Util;

namespace SpecterJS
{
	class Program
	{
		[STAThread]
		static int Main(string[] args)
		{
			var help = false;
			var options = new Options();
			var p = options.ToOptionSet();
			p.Add("config=", "Specifies JSON-formatted configuration file",
				v => options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(v)));

			p.Add("h|help", v => help = true);

			List<string> extra = p.Parse(args);

			if (help) return ShowHelp(p);

			var file = string.Empty;
			var code = string.Empty;

			if (extra.Any())
			{
				file = extra[0];
				if (!File.Exists(file))
					throw new FileNotFoundException();
				code = File.ReadAllText(file);
			}
			else
			{
				code = ResourceHelpers.ReadResource("repl.js");
                file = "repl.js";
			}

			try
			{
				Application.Run(new Specter(options).Execute(code, file, extra));
			}
			catch (ScriptInterruptedException) { }
			catch (ScriptEngineException e)
			{
				Console.WriteLine(e.ErrorDetails);
				return 1;
			}
			return Environment.ExitCode;
		}

		static int ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("  specterjs [options] [script] [argument [argument [...]]]");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
			return 0;
		}
	}
}
