(function (GLOBAL) {
	var fs, nativeCache = {};
	var cache = {};

	var nativeModules = {
		child_process: function () {
			if (!nativeCache['child_process'])
				nativeCache['child_process'] = phantom.createChildProcess();
			return nativeCache['child_process'];
		},
		cookiejar: function () {
			if (!nativeCache['cookiejar'])
				nativeCache['cookiejar'] = {
					create: phantom.createCookieJar
				};
			return nativeCache['cookiejar'];
		},
		fs: function () {
			if (!nativeCache['fs'])
				nativeCache['fs'] = phantom.createFilesystem();

			return nativeCache['fs'];
		},
		system: function () {
			if (!nativeCache['system'])
				nativeCache['system'] = phantom.createSystem();

			return nativeCache['system'];
		},
		webpage: function () {
			if (!nativeCache['webpage'])
				nativeCache['webpage'] = {
					create: phantom.createWebPage
				};

			return nativeCache['webpage'];
		},
		webserver: function () {
			if (!nativeCache['webserver'])
				nativeCache['webserver'] = {
					create: phantom.createWebServer
				};

			return nativeCache['webserver'];
		}
	};

    var extensions = {
        '.js': function(module, filename) {
            var code = fs.read(filename);
            module._compile(code);
        },

        '.json': function(module, filename) {
            module.exports = JSON.parse(fs.read(filename));
        }
    };
	
	function joinPath() {
		var args = Array.prototype.slice.call(arguments);
		return args.join('\\');
	}

	function dirname(path) {
		var replaced = path.replace(/\\[^\\]*\\?$/, '');
		if (replaced == path) {
			replaced = '';
		}
		return replaced;
	}

	function basename(path) {
		if (typeof path === "undefined")
			return path;

		return path.replace(/\\/, '/').replace(/.*\//, '');
	}

	var getPaths = function (request, dir) {
		var paths = [];
		if (request[0] == '.')
			paths.push(fs.absolute(dir ? joinPath(dir, request) : request));
		else if (fs.isAbsolute(request))
			paths.push(fs.absolute(request));
		else {
			while (dir) {
				paths.push(joinPath(dir, 'node_modules', request));
				dir = dirname(dir);
			}
		}
		return paths;
	};

	var tryFile = function (path) {
		if (fs.isFile(path)) return path;
		return null;
	};

	var tryExtensions = function (path) {
		var filename;
		for (var ext in extensions) {
			filename = tryFile(path + ext);
			if (filename) return filename;
		}
		return null;
	};

	var tryPackage = function (path) {
		var filename, pkg, packageFile = joinPath(path, 'package.json');
		if (fs.isFile(packageFile)) {
			pkg = JSON.parse(fs.read(packageFile));

			if (pkg && pkg.main) {
				filename = fs.absolute(joinPath(path, pkg.main));

				return tryFile(filename)
					|| tryExtensions(filename)
					|| tryExtensions(joinPath(filename, 'index'));
			}
		}

		return null;
	};

	var getFilename = function (request, dir) {
		var filename, paths = getPaths(request, dir);
		for (var i = 0; i < paths.length; i++) {
			var path = paths[i];
			filename = tryFile(path)
				|| tryExtensions(path)
				|| tryPackage(path)
				|| tryExtensions(joinPath(path, 'index'));

			if (filename) break;
		}
		return filename;
	};

	var loadModule = function (module, filename) {
		var ext = filename.match(/\.[^.]+$/)[0];
		if (!ext) ext = '.js';
		extensions[ext](module, filename);
	};

	function Module(filename, stubs) {
		if (filename) this._setFilename(filename);
		this.exports = {};
		this.stubs = {};
		for (var stub in stubs) {
			this.stubs[stub] = stubs[stub];
		}
	}

	Module.prototype._setFilename = function (filename) {
		this.id = this.filename = filename;
		this.dirname = dirname(filename);
	};

	Module.prototype._compile = function (code) {
		phantom.loadModule(code, this.filename);
	};

	Module.prototype._getRequire = function () {
		var self = this;

		function require(request) {
			return self.require(request);
		}
		require.cache = cache;
		require.extensions = extensions;
		//require.paths = paths;
		require.stub = function (request, exports) {
			self.stubs[request] = { exports: exports };
		};

		return require;
	};

	Module.prototype.require = function (request) {
		if (nativeModules[request]) {
			return nativeModules[request]();
		}

		if (this.stubs.hasOwnProperty(request)) {
			if (this.stubs[request].exports instanceof Function)
				this.stubs[request].exports = this.stubs[request].exports();
			return this.stubs[request].exports;
		}
		var filename = getFilename(request, this.dirname);
		if (!filename)
			throw new Error("Cannot find module '" + request + "'");

		if (cache.hasOwnProperty(filename)) {
			return cache[filename].exports;
		}

		var module = new Module(filename, this.stubs);
		cache[filename] = module;
		loadModule(module, filename);

		return module.exports;
	};
	fs = nativeModules.fs(), mainModule = new Module();
	GLOBAL.require = mainModule._getRequire();
	var cwd = fs.absolute(phantom.libraryPath);
	var mainFilename = joinPath(cwd, basename(require('system').args[0]) || 'repl');
	mainModule._setFilename(mainFilename);
})(this);

phantom.injectJs("console.js");
phantom.injectJs("timers.js");