function init() {
    var console = window.console = {
        log: function () {
            var message = "";
            var line = -1;
            var file = "";

            for (var i = 0; i < arguments.length; i++) {
                message += arguments[i];
            }

            try {
                throw new Error('dummy');
            }
            catch (e) {
                var stack = e.stack.split("\n");
                var match = /\((.+):(\d+):(\d+\))/g.exec(stack[2]);
                line = match[2];
                file = match[1];
            }
            external.ConsoleMessage(message, line, file);
        }
    };

    var alert = window.alert = function (msg) {
        external.Alert(msg);
    }

    var confirm = window.confirm = function (msg) {
        return external.Confirm(msg);
    };

    var callPhantom = window.callPhantom = function (data) {
        return external.CallPhantom(data);
    };
}