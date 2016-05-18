using System.IO;
using System.Reflection;

namespace SpecterJS.Util
{
	public static class ResourceHelpers
	{
		public static string ReadResource(string resourceName)
		{
			resourceName = resourceName
				.Replace('\\', '.')
				.Replace('/', '.');

			resourceName = Resources.ResourceManager.BaseName + "." + resourceName;

			using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)))
			{
				return stream.ReadToEnd();
			}
		}
	}
}
