using Microsoft.ClearScript;
using System;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.IO;

namespace SpecterJS.Bindings.Modules.ChildProcess
{
	public class Context : PropertyBag, IDisposable
	{
		private Process process;

		public Context(string libraryPath)
		{
			process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardInput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					WorkingDirectory = libraryPath
				},
				EnableRaisingEvents = true
			};

			StandardOutput = new Connection();
			StandardError = new Connection();
			Exit = new Connection();

			process.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
			{
				if (e.Data == null)
					Exit.Write(process.ExitCode);
				else
					StandardOutput.Write(e.Data);
			};

			process.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
			{
				if (e.Data != null)
					StandardError.Write(e.Data);
			};
		}

		[ScriptMember("_start")]
		public void Start(string cmd, dynamic args)
		{
			var arguments = new string[args.length];
			for (var i = 0; i < arguments.Length; ++i)
				arguments[i] = args[i];

			process.StartInfo.FileName = cmd;
			process.StartInfo.Arguments = string.Join(" ", arguments);
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
		}

		[ScriptMember("stdoutData")]
		public Connection StandardOutput { get; private set; }

		[ScriptMember("stderrData")]
		public Connection StandardError { get; private set; }

		[ScriptMember("exit")]
		public Connection Exit { get; private set; }

		[ScriptMember("_setEncoding")]
		public void SetEncoding(string encoding)
		{
			var enc = Encoding.GetEncodings()
				.Where(x => x.Name.Replace("-", "").Equals(encoding, StringComparison.OrdinalIgnoreCase))
				.Single()
				.GetEncoding();

			process.StartInfo.StandardErrorEncoding = enc;
			process.StartInfo.StandardOutputEncoding = enc;
		}

		[ScriptMember("_write")]
		public void Write(dynamic chunk, string encoding)
		{
			var enc = Encoding.GetEncodings()
				.Where(x => x.Name.Replace("-", "").Equals(encoding, StringComparison.OrdinalIgnoreCase))
				.Single()
				.GetEncoding();

			using (var writer = new StreamWriter(process.StandardInput.BaseStream, enc))
			{
				writer.Write(chunk);
			}
		}
		
		[ScriptMember("_close")]
		public void Dispose()
		{
			process.Dispose();
		}
	}
}
