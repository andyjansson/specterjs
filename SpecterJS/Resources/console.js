(function (global) {
	var system = require('system');
	var stdout = system.stdout;
	var stderr = system.stderr;

	function console() { }
	
	console.assert = function (assert) {
		var slicedArgs = Array.prototype.slice.call(arguments, 1);
		if (!assert)
			console.error(slicedArgs);
	};

	console.log = function () {
	    for (i = 0; i < arguments.length; i++) {
			switch (typeof arguments[i]) {
				case "function":
					stdout.write("[function " + arguments[i].name + "]");
					break;
				case "object":
					stdout.write("[object Object]");
					break;
				case "undefined":
					stdout.write("undefined");
					break;
				default:
					stdout.write(arguments[i].toString());
					break;
			}
		}
		stdout.write("\n");
		stdout.flush();
	};

	console.error = function () {
		for (i = 0; i < arguments.length; i++) {
			switch (typeof arguments[i]) {
				case "function":
					stderr.write("[function " + arguments[i].name + "]");
					break;
				case "object":
					stderr.write("[object Object]");
					break;
				case "undefined":
					stderr.write("undefined");
					break;
				default:
					stderr.write(arguments[i].toString());
					break;
			}
		}
		stderr.write("\n");

		var e = new Error();
		var stack = e.stack.split('\n');
		var trace;

		for (i = 1; i < stack.length; i++) {
			trace = stack[i];
			if (trace.includes('Function.console'))
				continue;
			
			trace = trace.replace("at ", "specterjs://code/");
			break;
		};

		stderr.writeLine("\n" + trace + " in global space\n");
		stderr.flush();
	};
	
	console.info = console.log;
	global.console = console;
})(this);