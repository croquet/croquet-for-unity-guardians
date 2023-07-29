// custom import script for an app using Croquet on WebView

import { StartSession } from "./unity-bridge";
import { MyModelRoot } from "./Models";
import { MyViewRoot, setLobbyWorker } from "./Views";

const worker = new Worker(new URL('./Lobby.js', import.meta.url));
setLobbyWorker(worker);

StartSession(MyModelRoot, MyViewRoot);
