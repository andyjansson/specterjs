using Microsoft.ClearScript;
using SpecterJS.Util;
using System.Collections.Generic;

namespace SpecterJS.Bindings
{
	public class Connection
	{
		private List<dynamic> callbacks;

		public Connection()
		{
			callbacks = new List<dynamic>();
		}

		[ScriptMember("connect")]
		public void Connect(dynamic cb)
		{
			callbacks.Add(cb);
		}

		[ScriptMember("disconnect")]
		public void Disconnect(dynamic cb)
		{
			callbacks.Remove(cb);
		}

		[NoScriptAccess]
		public void Write(params dynamic[] data)
		{
			foreach (var cb in callbacks)
			{
				ObjectHelpers.DynamicInvoke(cb, data);
			}
		}
	}
}
