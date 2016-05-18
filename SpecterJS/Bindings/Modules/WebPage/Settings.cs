using Microsoft.ClearScript;

namespace SpecterJS.Bindings.Modules.WebPage
{
	public class Settings
	{
		public Settings()
		{
			JavascriptEnabled = true;
			LoadImages = true;
			LocalToRemoteUrlAccessEnabled = false;

			XssAuditingEnabled = false;
			WebSecurityEnabled = true;
		}

		[ScriptMember(Name = "javascriptEnabled")]
		public bool JavascriptEnabled { get; set; }

		[ScriptMember(Name = "loadImages")]
		public bool LoadImages { get; set; }

		[ScriptMember(Name = "localToRemoteUrlAccessEnabled")]
		public bool LocalToRemoteUrlAccessEnabled { get; set; }

		[ScriptMember(Name = "userAgent")]
		public string UserAgent { get; set; }

		[ScriptMember(Name = "userName")]
		public string UserName { get; set; }

		[ScriptMember(Name = "password")]
		public string Password { get; set; }

		[ScriptMember(Name = "XSSAuditingEnabled")]
		public bool XssAuditingEnabled { get; set; }

		[ScriptMember(Name = "webSecurityEnabled")]
		public bool WebSecurityEnabled { get; set; }

		[ScriptMember(Name = "resourceTimeout")]
		public int ResourceTimeout { get; set; }
	}
}
