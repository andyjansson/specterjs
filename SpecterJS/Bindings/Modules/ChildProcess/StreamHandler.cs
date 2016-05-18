using Microsoft.ClearScript;
using SpecterJS.Util;
using System;

namespace SpecterJS.Bindings.Modules.ChildProcess
{
	public class StreamHandler
	{
		private dynamic callback;
		private Action start;

		public StreamHandler(Action start)
		{
			this.start = start;
		}

		[ScriptMember(Name = "on")]
		public void AddEvent(string evt, dynamic cb)
		{
			switch (evt)
			{
				case "data":
					callback = cb;
					start.Invoke();
					break;
			}
		}

		[NoScriptAccess]
		public void Write(string data)
		{
			if (data != null && data.Length > 0)
				ObjectHelpers.DynamicInvoke(callback, data);
		}
	}
}
