var system = require('system');
var stdin = system.stdin;
var stdout = system.stdout;

while (true) {
	try {
		stdout.write('specterjs> ');
		stdout.flush();
		var input = stdin.readLine();
		var output = eval(input);
		stdout.write('=> ');
		stdout.flush();
		console.log(output);
	}
	catch (e) {
		console.log(e.message);
	}
}