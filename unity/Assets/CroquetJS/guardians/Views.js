import { GameViewRoot } from "./unity-bridge";

export class MyViewRoot extends GameViewRoot {
}

export function setLobbyWorker(worker) {

    function askQuestion(n = 0) {
        console.log("Posting question to Lobby worker");
        worker.postMessage({
            question: 'The Answer to the Ultimate Question of Life, The Universe, and Everything.',
            n
        });
    }

    worker.onmessage = ({ data: { answer } }) => {
        console.log("Answer received from worker:", answer);
        setTimeout(() => askQuestion(answer), 1000);
    };

    setTimeout(askQuestion, 1000);
}
