using Newtonsoft.Json;
using SpecterJS.Browser;
using SpecterJS.CommandLine;
using System.Text;

namespace SpecterJS
{
	public class Options
	{
		public Options()
		{
			OutputEncoding = Encoding.Default;
			EmulationMode = IEVersion.Edge;
		}

		[Option(
			"ignore-ssl-errors=",
			Description = "Ignores SSL errors (expired/self-signed certificate errors): 'true' or 'false' (default)"
			)]
		[JsonProperty("ignoreSslErrors")]
		public bool IgnoreSslErrors { get; set; }

		[Option(
			"output-encoding=",
			Description = "Sets the encoding for the terminal output, default is 'utf8'"
			)]
		[JsonProperty("outputEncoding")]
		public Encoding OutputEncoding { get; set; }

		[Option(
			"emulation-mode=",
			Description = "Sets the emulation mode for Internet Explorer"
			)]
		[JsonProperty("emulationMode")]
		public IEVersion EmulationMode
		{
			set
			{
				Browser.EmulationMode.SetVersion(value);
			}
		}
	}
}
