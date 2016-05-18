using Microsoft.ClearScript;
using SpecterJS.Util;
using System;
using System.Diagnostics;

namespace SpecterJS.Bindings.Modules.ChildProcess
{
	public class Context : IDisposable
	{
		private Process process;
		private dynamic OnExit;
		private bool hasExited;
		private int status;

		public Context(string libraryPath, string cmd, string[] args)
		{
			process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					WorkingDirectory = libraryPath,
					FileName = cmd
				},
				EnableRaisingEvents = true
			};

			if (args != null)
				process.StartInfo.Arguments = string.Join(" ", args);


			StandardOutput = new StreamHandler(() => { process.BeginOutputReadLine(); });
			StandardError = new StreamHandler(() => { process.BeginErrorReadLine(); });

			process.Exited += delegate (object sender, EventArgs e)
			{
				status = process.ExitCode;
				hasExited = true;

				if (OnExit != null)
					ObjectHelpers.DynamicInvoke(OnExit, process.ExitCode);
			};

			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				StandardOutput.Write(e.Data);
			};

			process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				StandardError.Write(e.Data);
			};

			process.Start();
		}

		[ScriptMember("stdout")]
		public StreamHandler StandardOutput { get; set; }

		[ScriptMember("stderr")]
		public StreamHandler StandardError { get; set; }

		[ScriptMember("on")]
		public void AddEvent(string evt, dynamic cb)
		{
			switch (evt)
			{
				case "exit":
					OnExit = cb;
					if (hasExited)
						ObjectHelpers.DynamicInvoke(cb, status);
					break;
			}
		}

		[NoScriptAccess]
		public void Dispose()
		{
			process.Dispose();
		}
	}
}
