// custom import script for an app using Croquet on NodeJS

import { Worker } from 'node:worker_threads';

import { StartSession } from "./unity-bridge";
import { MyModelRoot } from "./Models";
import { MyViewRoot, setLobbyWorker } from "./Views";

const worker = new Worker(new URL('./Lobby-node.js', import.meta.url));
worker.on("message", data => worker.onmessage({data}));
setLobbyWorker(worker);

StartSession(MyModelRoot, MyViewRoot);
