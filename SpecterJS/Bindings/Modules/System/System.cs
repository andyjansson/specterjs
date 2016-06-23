using Microsoft.ClearScript;
using System;
using System.Collections;
using System.Threading;
using System.Text;
using SpecterJS.Bindings.Modules.FileSystem;

namespace SpecterJS.Bindings.Modules.System
{
	public class System
	{
		public System(dynamic args)
		{
			Arguments = args;
			OperatingSystem = new OperatingSystem();
		}

		[ScriptMember(Name = "args")]
		public dynamic Arguments { get; set; }

		[ScriptMember(Name = "env")]
		public IDictionary EnvironmentVariables
		{
			get
			{
				return Environment.GetEnvironmentVariables();
			}
		}

		[ScriptMember(Name = "os")]
		public OperatingSystem OperatingSystem { get; private set; }

		[ScriptMember(Name = "pid")]
		public int ProcessId
		{
			get
			{
				return Thread.CurrentThread.ManagedThreadId;
			}
		}

		[ScriptMember(Name = "platform")]
		public string Platform
		{
			get
			{
				return "specterjs";
			}
		}

		[ScriptMember(Name = "stdin")]
		public Stream StandardInput
		{
			get
			{
				return new Stream(new global::System.IO.StreamReader(global::System.Console.OpenStandardInput()));
			}
		}

		[ScriptMember(Name = "stdout")]
		public Stream StandardOutput
		{
			get
			{
				return new Stream(Console.Out);
			}
		}

		[ScriptMember(Name = "stderr")]
		public Stream StandardError
		{
			get
			{
				return new Stream(Console.Error);
			}
		}
	}
}
