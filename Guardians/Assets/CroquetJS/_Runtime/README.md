# CroquetJS Runtime Files

Croquet needs a JavaScript engine to execute JS code, and its supporting JS code. This folder contains both.

## Platforms

Depending on the host OS and build configuration, Croquet can use one of three ways to execute JavaScript:

### WebView

This is the preferred option. It uses the platform's own JS runtime via a WebView. This currently works on MacOS, iOS, and Android builds.

C# creates a Web Server and a WebView. The WebView loads `WebView/webview.html` from the Web Server. The HTML file loads the Croquet JS code. The JS code opens a WebSocket connection to the Web Server and thus establishes a communication channel between C# and JS.

### Node

On Windows, we launch a Web Server and a NodeJS process to execute JS code. The Croquet JS code opens a WebSocket connection to the Web Server and thus establishes a communication channel between C# and JS.

### WebGL

On WebGL builds, the Web Browser provides the JS execution context. It essentially loads `WebGL/index.html` which executes both the C# code that has been compiled to WASM, and the Croquet JS code. Since both run in the same JS context, they communicate directly, rather than via a WebSocket.

The build process copies this folder and a bundle of all the JS source code into `Assets/WebGLTemplates/CroquetLoader`, from which the regular Unity WebGL build continues.

## Packages

### `@croquet/unity-bridge`

This is the JS code implementing the communication between Unity and Croquet. It is loaded by the runtime engines mentioned above. In Croquet terms, it provides services to make C# able to act as Croquet Views (a.k.a. Pawns in Worldcore terms). For example, it exports `GameViewRoot`. The corresponding Models are in the next package.

### `@croquet/game-models`

This provides Croquet Models essential to the working of the Unity Bridge. Your application code imports at least `GameModelRoot` as the root model for your Croquet application.

### TODO

The original reason for splitting off `@croquet/game-models` into its own file was to be able to use it from both Unity and Non-Unity builds. We probably should merge them back together into one package.

Also, these JS packages should live in the Unity package because they depend on the Unity Bridge version.
