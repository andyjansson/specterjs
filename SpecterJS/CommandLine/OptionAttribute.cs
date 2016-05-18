using System;

namespace SpecterJS.CommandLine
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class OptionAttribute : Attribute
	{
		public string Prototype { get; private set; }
		public string Description { get; set; }
        
		public OptionAttribute(string prototype)
		{
			this.Prototype = prototype;
		}
	}
}
