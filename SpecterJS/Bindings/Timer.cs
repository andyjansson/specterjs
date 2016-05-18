using System;
using System.Threading;

namespace SpecterJS.Bindings
{
	public static class Timer
	{
		public static object Create(dynamic func, double delay, double period, object args)
		{
			var callback = new TimerCallback(state => func.apply(null, args));
			return new System.Threading.Timer(callback, null, Convert.ToInt64(delay), Convert.ToInt64(period));
		}
	}
}
