# SpecterJS

## THIS IS A WORK IN PROGRESS

SpecterJS is a scriptable headless browser with Internet Explorer bindings. SpecterJS aims to provide full API compatibility with [PhantomJS](http://phantomjs.org/).

# Compatibility

## Modules

| Module  | Status |
| -------- | ----- |
| Child Process  | Implemented  |
| Cookie Jar | Not Implemented |
| File System | Implemented |
| System | Implemented |
| Web page  | Partially implemented  |
| Web Server | Implemented |

## Command line Interface

### Usage
```
  specterjs [options] [script] [argument [argument [...]]]
```

**Note**: Executing SpecterJS without any arguments will engage Interactive Mode (REPL).

### Options

| Option | Status |
|--------|--------|
| --cookies-file= | Not implemented |
| --config= | Implemented |
| --debug= | Not implemented |
| --disk-cache= | Not implemented |
| --disk-cache-path= | Not implemented |
| --ignore-ssl-errors= | Implemented |
| --load-images= | Not implemented |
| --local-url-access= | Not implemented |
| --local-storage-path= | Not implemented |
| --local-storage-quota= | Not implemented |
| --offline-storage-path= | Not implemented |
| --offline-storage-quota= | Not implemented |
| --local-to-remote-url-access= | Not implemented |
| --max-disk-cache-size= | Not implemented |
| --output-encoding= | Implemented |
| --remote-debugger-port= | Not implemented |
| --remote-debugger-autoru=n | Not implemented |
| --proxy= | Not implemented |
| --proxy-auth= | Not implemented |
| --proxy-type= | Not implemented |
| --script-encoding= | Not implemented |
| --script-language= | Not implemented |
| --web-security= | Not implemented |
| --ssl-protocol= | Not implemented |
| --ssl-ciphers= | Not implemented |
| --ssl-certificates-path= | Not implemented |
| --ssl-client-certificate-file= | Not implemented |
| --ssl-client-key-file= | Not implemented |
| --ssl-client-key-passphrase= | Not implemented |
| --webdriver= | Not implemented |
| --webdriver-logfile= | Not implemented |
| --webdriver-loglevel= | Not implemented |
| --webdriver-selenium-grid-hub= | Not implemented |
| -w, --wd | Not implemented |
| -h, --help | Implemented |
| -v, --version | Not implemented |

#### Added options

| Option | Valid values | Description |
| ------ | ------ | ----------- |
| --emulation-mode= | `ie7`, `ie8`, `ie9`, `ie10`, `ie11` or `edge` | Sets the emulation mode for Internet Explorer. |

## JavaScript globals

| Global | Status | Comment |
| ------ | ------ | ------- |
| `phantom` | Implemented | —  |
| `require()` | Implemented | — |
| `console` | Partially implemented | `.log()`, `.error()` and `.info()` are implemented so far. |
| `setTimeout()`  | Implemented | — |
| `setInterval()` | Implemented | — |
| `clearTimeout()` | Implemented |—  |
| `clearInterval()` | Implemented | — |

# Contributing

If you find any bugs, please report them in the [issue tracker](https://github.com/andyjansson/specterjs/issues).

**Pull requests are welcome!**
