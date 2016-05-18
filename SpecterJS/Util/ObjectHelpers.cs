using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SpecterJS.Util
{
	public static class ObjectHelpers
	{
		private static readonly CSharpArgumentInfo argInfo = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null);
		public static object DynamicInvoke(object target, params object[] args)
		{
			var del = target as Delegate;
			if (del != null)
				return del.DynamicInvoke(args);
			var dynamicObject = target as DynamicObject;
			if (dynamicObject != null)
			{
				object result;

				if (args.Length == 0)
					return (target as dynamic)();

                var binder = Binder.Invoke(CSharpBinderFlags.None, null, Enumerable.Repeat(argInfo, args.Length));
				if (dynamicObject.TryInvoke((InvokeBinder)binder, args, out result))
					return result;
			}
			throw new InvalidOperationException("Invocation failed");
		}

        public static object GetProperty(object o, string member)
        {
            if (o == null) throw new ArgumentNullException("o");
            if (member == null) throw new ArgumentNullException("member");
            Type scope = o.GetType();
            IDynamicMetaObjectProvider provider = o as IDynamicMetaObjectProvider;
            if (provider != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(object));
                DynamicMetaObject mobj = provider.GetMetaObject(param);
                GetMemberBinder binder = (GetMemberBinder)Binder.GetMember(0, member, scope, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) });
                DynamicMetaObject ret = mobj.BindGetMember(binder);
                BlockExpression final = Expression.Block(
                    Expression.Label(CallSiteBinder.UpdateLabel),
                    ret.Expression
                );
                LambdaExpression lambda = Expression.Lambda(final, param);
                Delegate del = lambda.Compile();
                return del.DynamicInvoke(o);
            }
            else
            {
                return o.GetType().GetProperty(member, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(o, null);
            }
        }
    }
}
