import { parentPort } from "node:worker_threads";
import "./Lobby";

// Lobby uses globalThis.postMessage to send
// and sets globalThis.onmessage to receive

globalThis.postMessage = message => parentPort.postMessage(message);

parentPort.on("message", data => globalThis.onmessage({data}));
