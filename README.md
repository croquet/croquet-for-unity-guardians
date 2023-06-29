# Croquet for Unity Guardians

This repository contains a Croquet for Unity (C4U) port of David A Smith's "Guardians" game on Worldcore.

The most important directories are the following:
* `unity/` - a loadable Unity project, from which you can run the game in the Unity editor or build standalone versions for deployment on iOS and Android.
* `unity/Assets/Scenes/` - the main Guardians scene, and a SessionChooser scene (automatically included in builds) that allows a group of players to agree on a session number to join together.
* `unity/Assets/CroquetJS/` - JavaScript source for building the Croquet side of the game.  You can edit the code under this directory to change the game's behavior.

# Setup

## Repository Installation

To install the repository and prepare for building the Guardians game, carry out the following steps:

Install node.js and the node package manager (npm) for your platform here (LTS Recommended): https://nodejs.org/en/download


## Clone the Repo

```
git clone https://github.com/croquet/croquet-for-unity-guardians.git
```

Note: this repository's large size is predominantly due to our including a specific version of NodeJS for Windows.  On Windows we normally use NodeJS to run the JavaScript side of a C4U session, since Unity on Windows is currently unable to use the WebView mechanism that Croquet prefers.  On MacOS we use the WebView by default, but if a project has the necessary entry point for NodeJS execution (as the Guardians game does), NodeJS can be used on Mac as well.

## Load the Unity Project
For now, we **strongly recommend** using _exactly_ Unity Editor Version `2021.3.19f1` for C4U projects, to reduce the risk of confusing Unity when we publish updates. 2021.3.19f1 can be downloaded by pasting the following in your browser: `unityhub://2021.3.19f1/c9714fde33b6` (a deeplink to open hub to the correct version).

In the `Unity Hub` app, select `Open => Add project from disk`, then navigate to the `croquet-for-unity-guardians/unity` folder and hit `Add Project`.

**Note:** During this first loading, Unity might warn that there appear to be script errors. It's fine to hit `Ignore` and continue.  It appears to be related to the project's dependencies, and to be harmless.

## Install the JavaScript build tools and their dependencies

In the editor's top menu, go to the `Croquet` drop-down and select `Copy JS Build Tools`. This will copy some files into `Assets/CroquetJS`, and others into the root of the repository (i.e., the parent directory of the Unity project itself).

Now install the dependencies, in the repository root:

```
cd croquet-for-unity-guardians
npm install
```

**Note: whenever you download an update to the "Croquet Multiplayer" package using the package manager, you should immediately repeat this copy action and dependency installation.**

## Set up your Croquet Developer Credentials

In the Project Navigator (typically at bottom left), go to `Assets/Settings` and click `CroquetSettings.asset`.  The main field that you need to set up is the **Api Key**.

The API Key is a token of around 40 characters that you can create for yourself at https://croquet.io/account.  It provides access to the Croquet infrastructure.

The App Prefix is the way of identifying with your organization the Croquet apps that you develop and run.  The combination of this prefix and the App Name provided on the Croquet Bridge component in each scene is a full App ID - for example, `io.croquet.worldcore.guardians`.  For running the game it is fine to leave this prefix as is, but when you develop your own apps you must change the prefix so that the App ID is a globally unique identifier.  The ID must follow the Android reverse domain naming convention - i.e., each dot-separated segment must start with a letter, and only letters, digits, and underscores are allowed.

**For MacOS:** Find the Path to your Node executable, by going to a terminal and running
```
which node
```
On the Settings asset, fill in the **Path to Node** field with the path.

**For Windows:** Your system may complain about "Script Execution Policy" which will prevent our setup scripts from running. The following command allows script execution on Windows for the current user (respond **Yes to [A]ll** when prompted):
```
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

## Run the Game


In the Project Navigator, go to `Assets/Scenes` and double-click `Guardians.unity`.  If a "TMP importer" dialog comes up at this point, hit the top button ("Import TMP Essentials") then close the dialog.

In the editor's top menu, go to the `Croquet` drop-down and select `Build JS on Play` so that it has a check-mark next to it.

Press the play button.  Because this is the first time you have built the app, it will run a full webpack build of the JavaScript code - eventually writing webpack's log to the Unity console, each line prefixed with "JS builder".  You should then see console output for startup of the app - ending with "Croquet session ready", at which point the game should start to run.

## Debug with Chrome

On both MacOS and Windows, you can choose to use an external browser such as Chrome to run the JavaScript code.  For debugging, this is more convenient than letting the C4U bridge start up an invisible WebView.

In the Guardians scene (while play is stopped), select the "Croquet" object in the scene hierarchy (typically at top left), then in that object's "Croquet Runner" component select the **Wait For User Launch** checkbox.

Now whenever you press play, the console output will include a line of the form "ready for browser to load from http://localhost:...".  Copy that address (if you click on the line, it will appear as selectable text in the view below the console) then use it to launch a new browser tab.  This should complete the startup of the app.

When you stop play in the Unity editor, the browser tab will automatically leave the Croquet session.  If you restart play, you will need to reload the tab to join the session again.

# Questions
Please ask questions on our [discord](https://croquet.io/discord).

