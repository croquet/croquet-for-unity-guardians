mergeInto(LibraryManager.library, {
    RequestServerTime: function(walletAddressPtr) {
        var walletAddress = UTF8ToString(walletAddressPtr);
        if (typeof window.requestServerTimeFromWebpage === 'function') {
            window.requestServerTimeFromWebpage(walletAddress);
        } else {
            console.error('requestServerTimeFromWebpage is not defined');
        }
    },

    SubmitHighScore: function(messagePtr) {
        var message = UTF8ToString(messagePtr);
        if (typeof window.submitHighScoreFromWebpage === 'function') {
            window.submitHighScoreFromWebpage(message);
        } else {
            console.error('submitHighScoreFromWebpage is not defined');
        }
    }
});
