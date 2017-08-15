using System.IO;
using System.Reflection;
using System.Linq;

namespace SpecterJS.Util
{
	public static class ResourceHelpers
	{
		public static Stream ResourceAsStream(string resourceName)
		{
			resourceName = resourceName
				.TrimStart('\\', '/')
				.Replace('\\', '.')
				.Replace('/', '.');

			resourceName = Resources.ResourceManager.BaseName + "." + resourceName;
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}

		public static string ReadResource(string resourceName)
		{
			using (var stream = new StreamReader(ResourceAsStream(resourceName)))
			{
				return stream.ReadToEnd();
			}
		}

		public static bool ResourceExists(string resourceName)
		{
			resourceName = resourceName
				.TrimStart('\\', '/')
				.Replace('\\', '.')
				.Replace('/', '.');

			resourceName = Resources.ResourceManager.BaseName + "." + resourceName;
			return Assembly.GetExecutingAssembly().GetManifestResourceNames().Contains(resourceName);
		}
	}
}
