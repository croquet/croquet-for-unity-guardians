import { GetViewService } from "@croquet/worldcore-kernel";
import { GameViewRoot } from "./unity-bridge";

let lobbyWorker;

export function setLobbyWorker(worker) {
    lobbyWorker = worker;
}

export class MyViewRoot extends GameViewRoot {
    get gameViewManager() { return this._gameViewManager || (this._gameViewManager = GetViewService('GameViewManager')) }

    constructor(modelRoot) {
        super(modelRoot);

        function askQuestion(n = 0) {
            // console.log("Posting question to Lobby worker");
            lobbyWorker.postMessage({
                question: 'The Answer to the Ultimate Question of Life, The Universe, and Everything.',
                n
            });
        }

        lobbyWorker.onmessage = ({ data: { answer } }) => {
            // console.log("Answer received from worker:", answer);
            this.gameViewManager.forwardEventToUnity("lobby", "status", answer);
            setTimeout(() => askQuestion(answer), 1000);
        };

        setTimeout(askQuestion, 1000);
    }
}
