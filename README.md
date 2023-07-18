# Croquet for Unity: Guardians Demo

<img width="782" alt="image20" src="https://github.com/croquet/croquet-for-unity-guardians/assets/123010049/12602126-a465-478b-bfeb-2ca700a1202b">

This repository contains a Croquet for Unity (C4U) view of David A Smith's "Guardians" game built on Worldcore.

The most important directories are the following:
* `unity/` - the Unity project directory, from which you can run the game in the Unity editor or create builds for any platform Unity supports except WebGL.
* `unity/Assets/Scenes/` - the main Guardians scene, and a SessionChooser scene (automatically included in builds) that allows a group of players to agree on a session number to join together.
* `unity/Assets/CroquetJS/` - JavaScript source for building the Croquet side of the game. You can edit the code under this directory to change the game's behaviour.

# Setup
To setup the project take the following steps

## 1.0 Node Installation
Node is a prerequisite for installing JavaScript libraries like Croquet and Worldcore, as well as facilitating webpack builds.

Install node.js and the node package manager (npm) for your platform here (LTS Recommended): https://nodejs.org/en/download



## 2.0 Clone the Repo

```
git clone https://github.com/croquet/croquet-for-unity-guardians.git
```

Note: this repository's large size is predominantly due to our including a specific version of NodeJS for Windows.  On Windows we normally use NodeJS to run the JavaScript side of a C4U session, since Unity on Windows is currently unable to use the WebView mechanism that Croquet prefers.  On MacOS we use the WebView by default, but if a project has the necessary entry point for NodeJS execution (as the Guardians game does), NodeJS can be used on Mac as well.

## 3.0 Load the Unity Project

Make sure you have the Unity Hub installed from


 > **NOTE:** For now, we **strongly recommend** using _exactly_ Unity Editor Version `2021.3.19f1` for C4U projects

2021.3.19f1 can be downloaded by pasting the following in your browser: `unityhub://2021.3.19f1/c9714fde33b6` . This deeplink to the Unity Hub should open an installation dialog for the correct version.

In the `Unity Hub` app, select `Open => Add project from disk`, then navigate to the `croquet-for-unity-guardians/unity` folder and hit `Add Project`.

> **Note:** During this first loading, Unity might warn that there appear to be script errors. It's fine to hit `Ignore` and continue.  It appears to be related to the project's dependencies, and is determined to be harmless.

## 4.0 Install the JavaScript build tools and their dependencies

### 4.1 Copy Build Tools
In the editor's top menu, go to the `Croquet` drop-down and select `Copy JS Build Tools`. This will copy some files into `Assets/CroquetJS`, and others into the root of the repository (i.e., the parent directory of the Unity project itself).

### 4.2 Install JavaScript Dependencies
Now install the dependencies, in the repository root:

```
cd croquet-for-unity-guardians
npm install
```

> **Note:** any time an upgrade to the Croquet Multiplayer package is done, step 4.1 and 4.2 should be repeated to ensure the latest package's build tools and dependencies are available.


## 5.0 Set up your Croquet Developer Credentials

In the Project Navigator (typically at bottom left), go to `Assets/Settings` and click `CroquetSettings.asset`.  The main field that you need to set up is the **Api Key**.

The API Key is a token of around 40 characters that you can create for yourself at https://croquet.io/account.  It provides access to the Croquet infrastructure.

The App Prefix is the way of identifying with your organization the Croquet apps that you develop and run.  The combination of this prefix and the App Name provided on the `Croquet Bridge` component in each scene is a full App ID - for example, `io.croquet.worldcore.guardians`.  For running the game it is fine to leave this prefix as is, but when you develop your own apps you must change the prefix so that the App ID is a globally unique identifier.  The ID must follow the Android reverse domain naming convention - i.e., each dot-separated segment must start with a letter, and only letters, digits, and underscores are allowed.

**For MacOS:** Find the Path to your Node executable, by going to a terminal and running
```
which node
```
On the `CroquetSettings` asset, fill in the **Path to Node** field with the path.

**For Windows:** Your system may complain about "Script Execution Policy" which will prevent our setup scripts from running. The following command allows script execution on Windows for the current user (respond **Yes to [A]ll** when prompted):
```
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

## 6.0 Run the Game


In the Project Navigator, go to `Assets/Scenes` and double-click `Guardians.unity`.  If a "TMP importer" dialog comes up at this point, hit the top button ("Import TMP Essentials") then close the dialog. This is just part of the standard setup for Text Mesh Pro (which is used for all the UI).

In the editor's top menu, go to the `Croquet` drop-down and select `Build JS on Play` so that it has a check-mark next to it.

Press the play button.  Because this is the first time you have built the app, it will initiate a full webpack build of the JavaScript code - eventually writing webpack's log to the Unity console, each line prefixed with "JS builder".  You should then see console output for startup of the app - ending with "Croquet session running!", at which point the game should start to run.

### 6.1 Specifying a Croquet Session Name

_This is an optional configurability feature, not required for you to start playing with Guardians._

Croquet sessions are inherently multi-user, and this applies fully to the sessions that drive a C4U application. If you start the same application on multiple devices, you can expect that all those devices will be in the application together - for example, all cooperating in the Guardians game.

That said, the definition of what counts as "the same application" hinges on the application instances agreeing on _all_ the following factors:

1. **Application ID**. As mentioned in Section 5.0 above, this is a dot-separated name that in C4U is a concatenation of the **App Prefix** in the `CroquetSettings` asset and the **App Name** specified on the scene's `Croquet Bridge`. For example, `io.croquet.worldcore.guardians`.
2. **API Key**. Also mentioned in Section 5.0, this is a developer-specific key for using the Croquet infrastructure. In C4U this is specified in `CroquetSettings`. Note: strictly, the API keys do not have to be identical; as long as they were issued _for the same developer_ they will count as being in agreement.
3. **Session Name**. All Croquet sessions are launched with a Session Name, which in general can be any alphanumeric token - such as `helloworld`, or `123`. Given that the Application ID and API Key for a given app are unlikely to change frequently, Session Name is the most flexible way to control whether application instances will join the same session or not.

Our initial C4U applications - including Guardians - come with two alternative ways to specify the Session Name:

* **The Session Chooser scene**. Loading the scene `SessionChooser.unity` into the editor and pressing play will bring up a simple UI that allows you to select an integer (0 to 100) to act as the session's "name". Hitting the Start button in that UI then loads the Guardians scene, supplying the selected name.  _Note: for this scene hand-off to work, the Guardians scene must have the index `1`. This can be confirmed in the `Build Settings` dialog._

    The Session Chooser can optionally be included in a build (see "Making Sharable Builds" below).

* **"Default Session Name" property**. If the Guardians scene is _not_ started by way of the Session Chooser, C4U will use whatever value is found in the **Default Session Name** property of the scene's `Croquet Bridge`.


# Gameplay Details
Guardians is an instantly-joinable multiplayer game where you and your friends are defending your central spaceship from increasingly large groups of evil bots bent on destroying it!

## Controls
|Input|Alt. Input|Function|
|------------|-----|-------------------------------------------------------------|
| WASD       |     | Movement                                                    |
| Shift+WASD |     | Overdrive                                                   |
| Space      | LMB | Fire                                                        |
| U          |     | "Undying" mode                                              |
| M          |     | Mute All Audio                                              |
| G          |     | "God" mode camera                                           |
| Shift+G    |     | "God" mode camera (forces it for every user in the session) |
| 1-6        |     | Spawn Waves of 10-500 Bots                                  |


# Debugging Techniques
## Using a Web Browser to Debug the JavaScript Code

On both MacOS and Windows, you can choose to use an external browser such as Chrome to run the JavaScript code.  For debugging, this is more convenient than letting the C4U bridge start up an invisible WebView.

In the Guardians scene (while play is stopped), select the `Croquet` object in the scene hierarchy, then in that object's `Croquet Runner` component select the **Wait For User Launch** checkbox.

Now whenever you press play, the console output will include a line of the form "ready for browser to load from http://localhost:...".  Copy that address (if you click on the line, it will appear as selectable text in the view below the console) then use it to launch a new browser tab.  This should complete the startup of the app. All the JS developer tools (console, breakpoints etc) offered by the browser are available for working with the code.

When you stop play in the Unity editor, the browser tab will automatically leave the Croquet session.  If you restart play, you will need to reload the tab to join the session again.

## Viewing JS Errors in Unity
When _not_ running with an external browser, by default all JS console output in the "warn" and "error" categories will be transferred across the bridge and appear in the Unity console.

[July 2023] In the near future we plan to provide a configuration setting on the Unity Croquet object, to let the developer select which log categories are transferred.

# Making Sharable Builds

Before building the app to deploy for a chosen platform (e.g., Windows or MacOS standalone, or iOS or Android), there are some settings that you need to pay attention to:

* of course, there must be an **Api Key** present in `CroquetSettings.asset`
* the Build Settings dialog's **Scenes In Build** list can either include just the Guardians scene, or in addition the Session Chooser (which, if present, must be numbered scene 0). In the latter case, on startup the user will be forced to choose which session name to use.
* if the Session Chooser scene is not being included, ensure that the **Default Session Name** in the `Croquet Bridge` contains the alphanumeric token that you would like to use. For example, you might decide to build one version with the ID "playtest", that you distribute among your team during testing, and another with ID "presentation" that you use in presentations and distribute to the audience. Having the separate IDs means that people starting up one version cannot accidentally intrude on another.
* the `Croquet Bridge` **Use Node JS** checkbox _must be cleared_ for anything other than a Windows build
* all checkboxes under **Debug Logging Flags** should be cleared, so there is no wasteful logging happening behind the scenes
* the **Wait For User Launch** checkbox must be cleared

To ensure that the build will include your latest JavaScript code, you may wish to invoke `Build JS Now` on the `Croquet` drop-down (and confirm that the console messages show that the build succeeded).

Hit **Build**!

## Supplementary information for sharing MacOS builds

We have found that distributing a standalone MacOS build (`.app` file) requires some care to ensure that recipients can open it without being blocked by MacOS's security checks. One approach that we have found to work - there are doubtless others - is as follows:

1. Make the build - arriving at, say, a file `build.app`
2. In a terminal, execute the following command to replace the app's code signature
    `codesign --deep -s - -f /path/to/build.app`
3. Also use a terminal command (rather than the Finder) to zip the file, to ensure that the full directory structure is captured
    `tar -czf build.tgz /path/to/build.app`
4. Distribute the resulting `.tgz` file, **along with the following instructions to recipients**

    a. download this `.tgz` file

    b. double-click the `.tgz` to unpack the `.app` file

    c. **IMPORTANT: right-click (_not_ double-click)** the `.app` file and choose "Open"

    d. in the security dialog that appears, again choose "Open"

    e. if prompted to give permission for the app to access the network, agree.

# Questions
Please feel free to ask questions on our [discord](https://croquet.io/discord).

# Contribution
Contributions to the project are welcome as these projects are open source and we encourage community involvement.

1. Base your `feature/my-feature-name` branch off of `develop` branch
2. Make your changes
3. Open a PR against the `develop` branch
4. Discuss and Review the PR with the team
