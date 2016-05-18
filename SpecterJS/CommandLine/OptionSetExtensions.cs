using NDesk.Options;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SpecterJS.CommandLine
{
	public static class OptionSetExtensions
	{
		public static OptionSet ToOptionSet(this object obj)
		{
			var props = obj.GetType().GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof(OptionAttribute)));

			var options = new OptionSet();

			foreach (var prop in props)
			{
				var attr = prop.GetCustomAttribute<OptionAttribute>();
				Action<string> assign = value => prop.SetValue(obj, ConvertValue(prop.PropertyType, value), null);
				options.Add(attr.Prototype, attr.Description, assign);
			}

			return options;
		}

		private static object ConvertValue(Type t, string value)
		{
			if (t.IsEnum)
				return Enum.Parse(t, value, true);

			if (typeof(Encoding).IsAssignableFrom(t))
				return Encoding.GetEncodings()
					.Where(x => x.Name.Replace("-", "").Equals(value, StringComparison.OrdinalIgnoreCase))
					.Single().GetEncoding();

            return Convert.ChangeType(value, t);
        }
	}
}
