using Microsoft.ClearScript;
using System;

namespace SpecterJS.Bindings.Modules.System
{
	public class OperatingSystem
	{
		[ScriptMember(Name = "architecture")]
		public string Architecture
		{
			get
			{
				return Environment.Is64BitOperatingSystem ? "64bit" : "32bit";
			}
		}

		[ScriptMember(Name = "name")]
		public string Name
		{
			get
			{
				return "windows";
			}
		}

		[ScriptMember(Name = "version")]
		public string Version
		{
			get
			{
				var version = Environment.OSVersion.Version;
				switch (version.Major)
				{
					case 5: 
						switch (version.Minor)
						{
							case 0: return "2000";
							case 1: return "XP";
							default: return this.Architecture.Equals("64bit") ? "XP" : "2003";
						}
					case 6: 
						switch (version.Minor)
						{
							case 0: return "Vista";
							case 1: return "7";
							case 2: return "8";
							default: return "8.1";
						}
					default:
						return version.Major.ToString() + (version.Minor > 0 ? "." + version.Minor : "");
				}
            }
		}
	}
}
