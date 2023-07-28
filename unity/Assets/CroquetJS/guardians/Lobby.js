console.log("Lobby worker loaded");

globalThis.onmessage = ({ data: { question } }) => {
    console.log("Lobby worker received:", question);
    console.log("Lobby worker sending answer");
    globalThis.postMessage({
        answer: 42,
    });
};
