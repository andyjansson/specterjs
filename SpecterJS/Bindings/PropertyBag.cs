using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace SpecterJS.Bindings
{
	public abstract class PropertyBag : DynamicObject
	{
		private Dictionary<string, object> _dict;

		public PropertyBag()
			: base()
		{
			this._dict = new Dictionary<string, object>();
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var type = this.GetType();
			var propInfo = type.GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof(ScriptMemberAttribute)))
				.Where(p => p.GetCustomAttribute<ScriptMemberAttribute>().Name.Equals(binder.Name))
				.SingleOrDefault();

			if (propInfo != null)
			{
				result = propInfo.GetValue(this);
				return true;
			}
			return this._dict.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			var type = this.GetType();
			var propInfo = type.GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof(ScriptMemberAttribute)))
				.Where(p => p.GetCustomAttribute<ScriptMemberAttribute>().Name.Equals(binder.Name))
				.SingleOrDefault();

			if (propInfo != null)
				propInfo.SetValue(this, value);
			else
				this._dict[binder.Name] = value;

			return true;
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._dict.Keys;
		}
	}
}
