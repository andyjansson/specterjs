# `system` Module

| Property | Returns | Status | Notes |
| -------- | ------- | ------ | ----- |
| `args` | `string[]` | :white_check_mark: Implemented | The arguments the script was called with |
| `env` | `object` | :white_check_mark: Implemented | Environment variables |
| `os` | [Operating system](#operating-system) | :white_check_mark: Implemented | — |
| `pid` | `int` | :white_check_mark: Implemented | SpecterJS' process ID |
| `platform` | `string` | :white_check_mark: Implemented | returns `specterjs` |
| `stdin` | [Stream](fs.md#stream) | :white_check_mark: Implemented | — |
| `stdout` | [Stream](fs.md#stream) | :white_check_mark: Implemented | — |
| `stderr` | [Stream](fs.md#stream) | :white_check_mark: Implemented | — |

## Operating system

| Property | Returns | Status | Notes |
| -------- | ------- | ------ | ----- |
| `architecture` | `string` | :white_check_mark: Implemented | `32bit` or `64bit` |
| `windows` | `string` | :white_check_mark: Implemented | Always returns `windows` |
| `version` | `string` | :white_check_mark: Implemented | — |
