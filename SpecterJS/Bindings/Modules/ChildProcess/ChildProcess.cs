using Microsoft.ClearScript;

namespace SpecterJS.Bindings.Modules.ChildProcess
{
	public class ChildProcess : PropertyBag
	{
		private Phantom phantom;

		public ChildProcess(Phantom phantom)
		{
			this.phantom = phantom;
		}

		[ScriptMember(Name = "_createChildProcessContext")]
		public Context CreateContext()
		{
			return new Context(phantom.LibraryPath);
		}
	}
}
