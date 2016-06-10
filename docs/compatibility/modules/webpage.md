# `webpage` Module

| Method/Property | Type | Status | Notes |
| --------------- | ---- | ------ | ----- |
| `canGoBack` | `bool` | :white_check_mark: Implemented | — |
| `canGoForward` | `bool` | :white_check_mark: Implemented | — |
| `clipRect` | `object` | :white_check_mark: Implemented | — |
| `content` | `string` | :white_check_mark: Implemented | — |
| `cookies` | `object` | :x: Not implemented | — |
| `customHeaders` | `object` | :white_check_mark: Implemented | — |
| `event` | `object` | :x: Not implemented | — |
| `focusedFrameName` | `string` | :white_check_mark: Implemented | — |
| `frameContent` | `string` | :white_check_mark: Implemented | — |
| `frameName` | `string` |:white_check_mark: Implemented  | — |
| `framePlainText` | `string` | :white_check_mark: Implemented | — |
| `frameTitle` | `string` | :white_check_mark: Implemented | — |
| `frameUrl` | `string` | :white_check_mark: Implemented | — |
| `framesCount` | `int` | :white_check_mark: Implemented | — |
| `framesName` | `string[]` | :white_check_mark: Implemented | — |
| `libraryPath` | `string` | :white_check_mark: Implemented | — |
| `navigationLocked` | `bool` | :white_check_mark: Implemented | — |
| `offlineStoragePath` | `string` | :x: Not implemented | — |
| `offlineStorageQuota` | `int` | :x: Not implemented | — |
| `ownsPages` | `bool` | :x: Not implemented | — |
| `pages` | `object` | :x: Not implemented | — |
| `paperSize` | `object` | :x: Not implemented | — |
| `plainText` | `string` | :white_check_mark: Implemented | — |
| `scrollPosition` | `object` | :white_check_mark: Implemented | — |
| `settings` | `object` | :x: Not implemented | — |
| `title` | `string` | :white_check_mark: Implemented | — |
| `url` | `string` | :white_check_mark: Implemented | — |
| `viewportSize` | `object` | :white_check_mark: Implemented | — |
| `windowName` | `string` | :white_check_mark: Implemented | — |
| `zoomFactor` | `double` | :x: Not implemented | — |
| `addCookie(cookie)` | — | :x: Not implemented | — |
| `clearCookies()` | — | :x: Not implemented |—  |
| `close()` | — | :white_check_mark: Implemented | — |
| `deleteCookie(name)` | — | :x: Not implemented | — |
| `evaluateAsync(func[, delayMillis[, arg,[, ...]])` | — | :white_check_mark: Implemented | — |
| `evaluateJavaScript(str)` | — | :white_check_mark: Implemented | — |
| `evaluate(func[, arg[, ...]])` | — | :white_check_mark: Implemented | — |
| `getPage(windowName)` | `object` | :x: Not implemented | — |
| `go(index)` | — | :white_check_mark: Implemented | — |
| `goBack()` | — | :white_check_mark: Implemented | — |
| `goForward()` | — | :white_check_mark: Implemented | — |
| `includeJs(url, callback)` | — |:white_check_mark: Implemented  | — |
| `injectJs(filename)` | `bool` | :white_check_mark: Implemented | — |
| `openUrl(url, httpConf, settings)` | — | :x: Not implemented | — |
| `open(url[, callback])` | — | :white_check_mark: Implemented | — |
| `open(url, method[, callback])` | — | :white_check_mark: Implemented | — |
| `open(url, method, data[, callback])` | — | :white_check_mark: Implemented | — |
| `open(url, method, settings, callback)` | — | :x: Not implemented | — |
| `reload()` | — | :white_check_mark: Implemented | — |
| `renderBase64(format)` | — | :white_check_mark: Implemented | — |
| `renderBuffer(format, quality)` | — | :x: Not implemented | — |
| `render(filename[, settings])` | — | :warning: Partially implemented | `bmp`, `gif`, `jpg`, `png` implemented. Missing `pdf` and `ppm`. |
| `sendEvent(mouseEventType[, mouseX, mouseY, button='left'])` | — | :x: Not implemented | — |
| `sendEvent(keyboardEventType, keyOrKeys, [null, null, modifier])` | — | :white_check_mark: Implemented | — |
| `setContent(expectedContent, expectedUrl)` | — | :white_check_mark: Implemented | — |
| `stop()` | — | :white_check_mark: Implemented | — |
| `switchToFocusedFrame()` | — | :white_check_mark: Implemented | — |
| `switchToFrame(name)` | — | :white_check_mark: Implemented | — |
| `switchToFrame(position)` | — | :white_check_mark: Implemented | — |
| `switchToMainFrame()` | — | :white_check_mark: Implemented | — |
| `switchToParentFrame()` | — | :white_check_mark: Implemented | — |
| `uploadFile(selector, filename)` | — | :x: Not implemented | — |
| `closing(page)` |  | :x: Not implemented | — |
| `initialized()` |  | :x: Not implemented | — |
| `javaScriptAlertSent(message)` |  | :x: Not implemented | — |
| `javaScriptConsoleMessageSent(message)` |  | :x: Not implemented | — |
| `loadFinished(status)` |  | :x: Not implemented | — |
| `loadStarted()` |  | :x: Not implemented | — |
| `navigationRequested(url, navigationType, navigationLocked, isMainFrame)` |  | :x: Not implemented | — |
| `rawPageCreated(page)` |  | :x: Not implemented | — |
| `resourceError(resource)` |  | :x: Not implemented | — |
| `resourceReceived(request)` |  | :x: Not implemented | — |
| `resourceRequested(request)` |  | :x: Not implemented | — |
| `urlChanged(url)` |  | :x: Not implemented | — |
| `onAlert` | `function` | :white_check_mark: Implemented | — |
| `onCallback` | `function` | :white_check_mark: Implemented | — |
| `onClosing` | `function` | :x: Not implemented | — |
| `onConfirm` | `function` | :white_check_mark: Implemented |— |
| `onConsoleMessage` | `function` | :white_check_mark: Implemented | — |
| `onError` | `function` | :white_check_mark: Implemented | — |
| `onFilePicker` | `function` | :x: Not implemented | — |
| `onInitialized` | `function` | :white_check_mark: Implemented | — |
| `onLoadFinished` | `function` | :white_check_mark: Implemented | — |
| `onLoadStarted` | `function` | :white_check_mark: Implemented | — |
| `onNavigationRequested` | `function` | :white_check_mark: Implemented | — |
| `onPageCreated` | `function` | :x: Not implemented | — |
| `onPrompt` | `function` | :white_check_mark: Implemented | — |
| `onResourceError` | `function` | :x: Not implemented | — |
| `onResourceReceived` | `function` | :x: Not implemented | — |
| `onResourceRequested` | `function` | :x: Not implemented | — |
| `onResourceTimeout` | `function` | :x: Not implemented | — |
| `onUrlChanged` | `function` | :white_check_mark: Implemented | — |
