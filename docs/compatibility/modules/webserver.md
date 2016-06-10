# `webserver` Module

| Method/Property | Returns | Status |
| --------------- | ------- | ------ |
| `listen(port, callback)` | [Listener](#listener) | :white_check_mark: Implemented |
| `listen(addr, callback)` | [Listener](#listener) | :white_check_mark: Implemented |
| `listen(port, opts, callback)` | [Listener](#listener) | :white_check_mark: Implemented |
| `port` | `int` | :white_check_mark: Implemented |

## Listener

| Method | Returns | Status |
| ------ | ------- | ------ |
| `close()` | — | :white_check_mark: Implemented |

## `callback` function

| Parameter |
| ------ |
| [Request](#request) |
| [Response](#response) |

## Request

| Property | Returns | Status |
| -------- | ------- | ------ |
| `method` | `string` | :white_check_mark: Implemented |
| `url` | `string` | :white_check_mark: Implemented |
| `httpVersion` | `string` | :white_check_mark: Implemented |
| `headers` | `object` | :white_check_mark: Implemented |
| `post` | `string` | :white_check_mark: Implemented |
| `postRaw` | `string` | :white_check_mark: Implemented |

## Response

| Method/Property | Returns | Status |
| --------------- | ------- | ------ |
| `headers` | `object` | :white_check_mark: Implemented |
| `statusCode` | `int` | :white_check_mark: Implemented |
| `setHeader(name, value)` | — | :white_check_mark: Implemented |
| `header(name)` | `string` | :white_check_mark: Implemented |
| `setEncoding(encoding)` | — | :white_check_mark: Implemented |
| `write(data)` | — | :white_check_mark: Implemented |
| `writeHead(statusCode, headers)` | — | :white_check_mark: Implemented |
| `close()` | — | :white_check_mark: Implemented |
| `closeGracefully()` | — | :white_check_mark: Implemented |
