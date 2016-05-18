using Microsoft.ClearScript;
using System;
using System.IO;
using System.Linq;

namespace SpecterJS.Bindings.Modules.FileSystem
{
	public class FileSystem
	{
		[ScriptMember(Name = "separator")]
		public string Separator
		{
			get
			{
				return Path.DirectorySeparatorChar.ToString();
			}
		}

		[ScriptMember(Name = "workingDirectory")]
		public string WorkingDirectory
		{
			get
			{
				return Environment.CurrentDirectory;
			}
		}

		[ScriptMember(Name = "absolute")]
		public string GetAbsolutePath(string path)
		{
			return Path.GetFullPath(path);
		}

		[ScriptMember(Name = "changeWorkingDirectory")]
		public bool ChangeWorkingDirectory(string path)
		{
			if (Directory.Exists(path))
			{
				Environment.CurrentDirectory = path;
				return true;
			}
			return false;
		}

		[ScriptMember(Name = "copy")]
		public void Copy(string source, string destination)
		{
			File.Copy(source, destination);
		}

		[ScriptMember(Name = "copyTree")]
		public void CopyTree(string source, string destination)
		{
			if (Directory.Exists(source) && !String.IsNullOrEmpty(destination))
			{
				if (destination.IndexOfAny(Path.GetInvalidPathChars()) < 0)
				{
					foreach (var dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
						Directory.CreateDirectory(dir.Replace(source, destination));

					foreach (var file in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
						File.Copy(file, file.Replace(source, destination), true);
				}
			}
		}

		[ScriptMember(Name = "exists")]
		public bool Exists(string path)
		{
			return File.Exists(path) || Directory.Exists(path);
		}

		[ScriptMember(Name = "isAbsolute")]
		public bool IsAbsolute(string path)
		{
			if (Directory.Exists(path) || File.Exists(path))
			{
				return Path.IsPathRooted(path);
			}
			return false;
		}

		[ScriptMember(Name = "isDirectory")]
		public bool IsDirectory (string path)
		{
			return Directory.Exists(path);
		}

		[ScriptMember(Name = "isExecutable")]
		public bool IsExecutable(string path)
		{
			var exec = new string[] { ".exe", ".bat", ".com" }; 
			string ext = Path.GetExtension(path).ToLower();
			return exec.Contains(ext);
		}

		[ScriptMember(Name = "isFile")]
		public bool IsFile(string path)
		{
			return File.Exists(path);
		}

		[ScriptMember(Name = "isLink")]
		public bool IsLink(string path)
		{
			string directory = Path.GetDirectoryName(path);
			string file = Path.GetFileName(path);

			Shell32.Shell shell = new Shell32.Shell();
			Shell32.Folder folder = shell.NameSpace(directory);
			Shell32.FolderItem folderItem = folder.ParseName(file);

			return folderItem?.IsLink ?? false;
		}

		[ScriptMember(Name = "isReadable")]
		public bool IsReadable(string path)
		{
			try
			{
				using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
				{
					return true;
				}
			}
			catch { }
			return false;
		}

		[ScriptMember(Name = "isWritable")]
		public bool IsWritable(string path)
		{
			try
			{
				using (var fs = File.Open(path, FileMode.Open, FileAccess.Write))
				{
					return true;
				}
			}
			catch { }
			return false;
		}

		[ScriptMember(Name = "list")]
		public string[] GetFiles(string path)
		{
			return Directory.Exists(path) ? Directory.GetFiles(path) : new string[] { };
		}

		[ScriptMember(Name ="makeDirectory")]
		public bool MakeDirectory (string path)
		{
			if (Directory.Exists(path))
				return false;

			try
			{
				Directory.CreateDirectory(path);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		[ScriptMember(Name = "makeTree")]
		public bool MakeDirectoryTree(string path)
		{
			if (Directory.Exists(path))
				return true;

			try
			{
				Directory.CreateDirectory(path);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		[ScriptMember(Name = "move")]
		public void MoveFile(string source, string destination)
		{
			File.Move(source, destination);
		}

		[ScriptMember(Name = "open")]
		public Stream Open(string path, dynamic mode)
		{
			return new Stream(path, mode);
		}

		[ScriptMember(Name = "read")]
		public string Read(string path, string param = "")
		{
			if (!param.Contains("r"))
				param += "r";

			using (var stream = Open(path, param))
			{
				return stream.Read();
			}
		}

		[ScriptMember(Name = "readLink")]
		public string ReadLink(string path)
		{
			string directory = Path.GetDirectoryName(path);
			string file = Path.GetFileName(path);
			Shell32.Shell shell = new Shell32.Shell();
			Shell32.Folder folder = shell.NameSpace(directory);
			Shell32.FolderItem folderItem = folder.ParseName(file);
			Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
			return link.Path;
		}

		[ScriptMember(Name = "remove")]
		public void RemoveFile(string path)
		{
			File.Delete(path);
		}

		[ScriptMember(Name = "removeDirectory")]
		public void RemoveDirectory(string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path);
		}

		[ScriptMember(Name = "removeTree")]
		public void RemoveTree(string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);

			if (File.Exists(path))
				File.Delete(path);
		}

		[ScriptMember(Name = "size")]
		public long GetFileSize(string path)
		{
			return File.Exists(path) ? new FileInfo(path).Length : -1;
		}

		[ScriptMember(Name = "touch")]
		public void TouchFile(string path)
		{
			if (!File.Exists(path))
				File.Create(path);

			File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
		}

		[ScriptMember(Name = "write")]
		public void Write(string path, string content, string param)
		{
			if (!param.Contains("w"))
				param += "w";

			using (var stream = Open(path, param))
			{
				stream.Write(content);
			}
		}
	}
}
