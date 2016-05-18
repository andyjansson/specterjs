using System.Runtime.Serialization;

namespace SpecterJS.Browser
{
	public enum IEVersion
	{
		[EnumMember(Value = "ie7")]
		IE7 = 0x1B58,

		[EnumMember(Value = "ie8")]
		IE8 = 0x22B8,

		[EnumMember(Value = "ie9")]
		IE9 = 0x270F,

		[EnumMember(Value = "ie10")]
		IE10 = 0x02710,

		[EnumMember(Value = "ie11")]
		IE11 = 0x2AF8,

		[EnumMember(Value = "edge")]
		Edge = 0x2AF9
	}
}
