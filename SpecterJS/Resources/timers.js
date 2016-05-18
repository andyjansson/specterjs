(function (global) {
	function createTimer(periodic, func, delay) {
		var period = periodic ? delay : -1;
		var args = Array.prototype.slice.call(arguments, 3);
		return Timer.Create(func, delay, period, args);
	};

	global.setTimeout = createTimer.bind(null, false);
	global.setInterval = createTimer.bind(null, true);
	global.clearTimeout = function (id) { id.Dispose(); };
	global.clearInterval = clearTimeout;
})(this);