# `child_process` Module

| Method | Returns | Status |
| ------ | ------- | ------ |
| `spawn(cmd, args [, opts])` | [Subprocess](#subprocess) |:white_check_mark: Implemented |
| `execFile(cmd, args, opts, callback)` | — | :white_check_mark: Implemented |

## Subprocess

| Method/Property | Returns | Status | Notes |
| --------------- | ------- | ------ | ----- |
| `stdout` | [Stream handler](#stream-handler) | :white_check_mark: Implemented | — |
| `stderr` | [Stream handler](#stream-handler) | :white_check_mark: Implemented | — |
| `on(event)` | — | :white_check_mark: Implemented | Listens to the `exit` event |

## Stream handler

| Method | Returns | Status | Notes |
| ------ | ------- | ------ | ----- |
| `on(event)` | — | :white_check_mark: Implemented | Listens to the `data` event |
