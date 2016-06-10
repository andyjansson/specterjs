# `fs` Module

| Method/Property | Returns | Status | Notes |
| --------------- | ------- | ------ | ----- |
| `separator` | `string` | :white_check_mark: Implemented | The directory separator, i.e. `\` |
| `workingDirectory` | `string` | :white_check_mark: Implemented | Returns the current working directory |
| `absolute(path)` | `string` | :white_check_mark: Implemented | — |
| `changeWorkingDirectory(path)` | `bool` | :white_check_mark: Implemented | — |
| `copy(source, destination)` | — | :white_check_mark: Implemented | — |
| `copyTree(source, destination)` | — | :white_check_mark: Implemented | — |
| `exists(path)` | `bool` | :white_check_mark: Implemented | — |
| `isAbsolute(path)` | `bool` | :white_check_mark: Implemented | — |
| `isDirectory(path)` | `bool` | :white_check_mark: Implemented | — |
| `isExecutable(path)` | `bool` | :white_check_mark: Implemented | — |
| `isFile(path)` | `bool` | :white_check_mark: Implemented | — |
| `isLink(path)` | `bool` | :white_check_mark: Implemented | — |
| `isReadable(path)` | `bool` | :white_check_mark: Implemented | — |
| `isWritable(path)` | `string` | :white_check_mark: Implemented | — |
| `list(path)` | `string[]` | :white_check_mark: Implemented | Lists files in path |
| `makeTree(path)` | `bool` | :white_check_mark: Implemented | — |
| `move(source, destination)` | — | :white_check_mark: Implemented | — |
| `open(path, mode)` | [Stream](#stream) | :white_check_mark: Implemented | — |
| `read(path[, param])` | `string` | :white_check_mark: Implemented | — |
| `readLink(path)` | `string` | :white_check_mark: Implemented | — |
| `remove(path)` | — | :white_check_mark: Implemented | — |
| `removeDirectory(path)` | — | :white_check_mark: Implemented | — |
| `removeTree(path)` | — | :white_check_mark: Implemented | — |
| `size(path)` | `long` | :white_check_mark: Implemented | Returns the file size of the provided path |
| `touch(path)` | — | :white_check_mark: Implemented | — |
| `write(path, content, param)` | — | :white_check_mark: Implemented | — |

## Stream

| Method/Property | Returns | Status |
| --------------- | ------- | ------ |
| `atEnd()` | `bool` | :white_check_mark: Implemented |
| `close()` | — | :white_check_mark: Implemented |
| `flush()` | — | :white_check_mark: Implemented |
| `read()` | `string` | :white_check_mark: Implemented |
| `readLine()` | `string` | :white_check_mark: Implemented |
| `seek(pos)` | — | :white_check_mark: Implemented |
| `write(data)` | — | :white_check_mark: Implemented |
| `writeLine(data)` | — | :white_check_mark: Implemented |
