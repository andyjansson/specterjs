using Microsoft.Win32;
using System;

namespace SpecterJS.Browser
{
	public static class EmulationMode
	{
		public static void SetVersion(IEVersion version)
		{
			string regKey = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
			string application = AppDomain.CurrentDomain.FriendlyName;

			using (RegistryKey reg2 = Registry.CurrentUser.CreateSubKey(regKey))
			{
				reg2.SetValue(application, version, RegistryValueKind.DWord);
				reg2.Close();
			}
		}
	}
}
