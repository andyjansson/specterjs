using Microsoft.ClearScript;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SpecterJS.Bindings.Modules.FileSystem
{
	public class Stream : IDisposable
	{
		private StreamReader reader;
		private StreamWriter writer;

		public Stream(StreamReader reader)
		{
			this.reader = reader;
		}

		public Stream(StreamWriter writer)
		{
			this.writer = writer;
		}

		public Stream(StreamReader reader, StreamWriter writer)
		{
			this.reader = reader;
			this.writer = writer;
		}

		public Stream(string path, dynamic opt)
		{
			var mode = string.Empty;
			var charset = "utf-8";
			var type = opt.GetType();

			if (type == typeof(string))
				mode = opt as string;
			else
			{
				foreach (var name in opt.GetDynamicMemberNames())
				{
					switch ((string)name)
					{
						case "mode":
							mode = opt[name];
							break;
						case "charset":
							charset = opt[name];
							break;
					}
				}
			}

			bool binary = false;
			FileMode fileMode = FileMode.OpenOrCreate;
			FileAccess fileAccess = new FileAccess();
			foreach (char c in mode.ToLower())
			{
				switch (c)
				{
					case 'r':
						fileAccess |= FileAccess.Read;
						break;
					case 'w':
						fileAccess |= FileAccess.Write;
						break;
					case 'a':
					case '+':
						fileMode |= FileMode.Append;
						break;
					case 'b':
						binary = true;
						break;
				}
			}

			Encoding encoding = null;
			if (!binary)
				encoding = Encoding.GetEncodings()
					.Where(x => x.Name.Equals(charset, StringComparison.OrdinalIgnoreCase))
					.Single().GetEncoding();

			var file = File.Open(path, fileMode, fileAccess);
			if (fileAccess.HasFlag(FileAccess.Read))
				reader = new StreamReader(file, encoding);

			if (fileAccess.HasFlag(FileAccess.Write))
				writer = new StreamWriter(file, encoding);
		}

		[ScriptMember(Name = "atEnd")]
		public bool AtEnd()
		{
			if (reader != null)
				return reader.BaseStream.Length == reader.BaseStream.Position;

			return true;
		}

		[ScriptMember(Name = "close")]
		public void Close()
		{
			reader?.Close();
			writer?.Close();
		}

		[NoScriptAccess]
		public void Dispose()
		{
			this.Close();
		}

		[ScriptMember(Name = "flush")]
		public void Flush()
		{
			writer?.Flush();
		}

		[ScriptMember(Name = "read")]
		public string Read()
		{
			return reader?.ReadToEnd();
		}

		[ScriptMember(Name = "readLine")]
		public string ReadLine()
		{
			return reader?.ReadLine();
		}

		[ScriptMember(Name = "seek")]
		public void Seek(int pos)
		{
			reader?.BaseStream.Seek(pos, SeekOrigin.Begin);
		}

		[ScriptMember(Name = "write")]
		public void Write(string data)
		{
			writer?.Write(data);
		}

		[ScriptMember(Name = "writeLine")]
		public void WriteLine(string data)
		{
			writer?.WriteLine(data);
		}
	}
}
