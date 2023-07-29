console.log("Lobby worker loaded");

globalThis.onmessage = ({ data: { question, n } }) => {
    console.log("Lobby worker received:", question, n);
    console.log("Lobby worker sending answer");
    globalThis.postMessage({
        answer: n ? n + 1 : 42,
    });
};
