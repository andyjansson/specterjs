using Microsoft.ClearScript;
using System;
using System.Collections;
using System.Threading;
using SpecterJS.Bindings.Modules.FileSystem;

namespace SpecterJS.Bindings.Modules.System
{
	public class System : PropertyBag
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

		[ScriptMember(Name = "standardin")]
		public Stream StandardInput
		{
			get
			{
				return new Stream(new global::System.IO.StreamReader(Console.OpenStandardInput()));
			}
		}

		[ScriptMember(Name = "standardout")]
		public Stream StandardOutput
		{
			get
			{
				return new Stream(Console.Out);
			}
		}

		[ScriptMember(Name = "standarderr")]
		public Stream StandardError
		{
			get
			{
				return new Stream(Console.Error);
			}
		}
	}
}
