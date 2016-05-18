using Microsoft.ClearScript;
using SpecterJS.Util;
using System.Dynamic;
using System.Text;

namespace SpecterJS.Bindings.Modules.ChildProcess
{
	public class ChildProcess
	{
		private Phantom phantom;

		public ChildProcess(Phantom phantom)
		{
			this.phantom = phantom;
		}

		[ScriptMember(Name = "spawn")]
		public Context Spawn(string cmd, string args = "", dynamic opts = null)
		{
			return Spawn(cmd, new string[] { args }, opts);
		}

		[ScriptMember(Name = "spawn")]
		public Context Spawn(string cmd, string[] args, dynamic opts = null)
		{
			var context = new Context(phantom.LibraryPath, cmd, args);
			return context;
		}

		[ScriptMember(Name = "execFile")]
		public void ExecFile(string cmd, string[] args, dynamic opts, DynamicObject cb)
		{
			var context = Spawn(cmd, args, opts);
			var stdout = new StringBuilder();
			var stderr = new StringBuilder();

			OnStdErr stdoutHandler = delegate (string data)
			{
				stdout.Append(data);
			};
			OnStdErr stderrHandler = delegate (string data)
			{
				stderr.Append(data);
			};

			onExit exitHandler = delegate (int status)
			{
                ObjectHelpers.DynamicInvoke(cb, status, stdout.ToString(), stderr.ToString());
			};
			context.StandardOutput.AddEvent("data", stdoutHandler);
			context.StandardError.AddEvent("data", stderrHandler);
			context.AddEvent("exit", exitHandler);
		}

		private delegate void OnStdOut(string data);
		private delegate void OnStdErr(string data);
		private delegate void onExit(int status);
	}
}
