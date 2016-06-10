# SpecterJS

## THIS IS A WORK IN PROGRESS

SpecterJS is a scriptable headless browser with Internet Explorer bindings. SpecterJS aims to provide full API compatibility with [PhantomJS](http://phantomjs.org/).

# Compatibility

## Modules

| Module  | Status |
| -------- | ----- |
| [`child_process`](docs/compatibility/modules/child_process.md) | :white_check_mark: Implemented  |
| `cookiejar` | :x: Not Implemented |
| [`fs`](docs/compatibility/modules/fs.md) | :white_check_mark: Implemented |
| [`system`](docs/compatibility/modules/system.md) | :white_check_mark: Implemented |
| [`webpage`](docs/compatibility/modules/webpage.md)  | :warning: Partially implemented  |
| [`webserver`](docs/compatibility/modules/webserver.md) | :white_check_mark: Implemented |

## Command line Interface

### Usage
```
  specterjs [options] [script] [argument [argument [...]]]
```

**Note**: Executing SpecterJS without any arguments will engage Interactive Mode (REPL).

### Options

| Option | Status |
|--------|--------|
| --cookies-file= | :x: Not implemented |
| --config= | :white_check_mark: Implemented |
| --debug= | :x: Not implemented |
| --disk-cache= | :x: Not implemented |
| --disk-cache-path= | :x: Not implemented |
| --ignore-ssl-errors= | :white_check_mark: Implemented |
| --load-images= | :x: Not implemented |
| --local-url-access= | :x: Not implemented |
| --local-storage-path= | :x: Not implemented |
| --local-storage-quota= | :x: Not implemented |
| --offline-storage-path= | :x: Not implemented |
| --offline-storage-quota= | :x: Not implemented |
| --local-to-remote-url-access= | :x: Not implemented |
| --max-disk-cache-size= | :x: Not implemented |
| --output-encoding= | :white_check_mark: Implemented |
| --remote-debugger-port= | :x: Not implemented |
| --remote-debugger-autoru=n | :x: Not implemented |
| --proxy= | :x: Not implemented |
| --proxy-auth= | :x: Not implemented |
| --proxy-type= | :x: Not implemented |
| --script-encoding= | :x: Not implemented |
| --script-language= | :x: Not implemented |
| --web-security= | :x: Not implemented |
| --ssl-protocol= | :x: Not implemented |
| --ssl-ciphers= | :x: Not implemented |
| --ssl-certificates-path= | :x: Not implemented |
| --ssl-client-certificate-file= | :x: Not implemented |
| --ssl-client-key-file= | :x: Not implemented |
| --ssl-client-key-passphrase= | :x: Not implemented |
| --webdriver= | :x: Not implemented |
| --webdriver-logfile= | :x: Not implemented |
| --webdriver-loglevel= | :x: Not implemented |
| --webdriver-selenium-grid-hub= | :x: Not implemented |
| -w, --wd | :x: Not implemented |
| -h, --help | :white_check_mark: Implemented |
| -v, --version | :x: Not implemented |

#### Added options

| Option | Valid values | Description |
| ------ | ------ | ----------- |
| --emulation-mode= | `ie7`, `ie8`, `ie9`, `ie10`, `ie11` or `edge` | Sets the emulation mode for Internet Explorer. |

## JavaScript globals

| Global | Status | Comment |
| ------ | ------ | ------- |
| `phantom` | :white_check_mark: Implemented | —  |
| `require()` | :white_check_mark: Implemented | — |
| `console` | :warning: Partially implemented | `.log()`, `.error()` and `.info()` are implemented so far. |
| `setTimeout()`  | :white_check_mark: Implemented | — |
| `setInterval()` | :white_check_mark: Implemented | — |
| `clearTimeout()` | :white_check_mark: Implemented |—  |
| `clearInterval()` | :white_check_mark: Implemented | — |

# Contributing

If you find any bugs, please report them in the [issue tracker](https://github.com/andyjansson/specterjs/issues).

**Pull requests are welcome!**
