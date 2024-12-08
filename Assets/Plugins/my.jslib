let tonConnectUI;

mergeInto(LibraryManager.library, {
    jsInit: function () {
        tonConnectUI = new TON_CONNECT_UI.TonConnectUI({
            manifestUrl: 'https://tops-bullfrog-painfully.ngrok-free.app/tonconnect-manifest.json'
        });

        try {
            const initData = window.Telegram.WebApp.initDataUnsafe;
            const initDataString = JSON.stringify(initData);
            window.unityInstanceRef.SendMessage('TonConnectHandler', 'OnLaunchDataReceived', initDataString);
        } catch (error) {
            window.unityInstanceRef.SendMessage('TonConnectHandler', 'OnLaunchDataReceived', 'Error');
        }
    },
    
    jsConnectWallet: function () {
        tonConnectUI.connectWallet()
            .then((connectedWallet) => {
                const connectedWalletString = JSON.stringify(connectedWallet);
                window.unityInstanceRef.SendMessage('TonConnectHandler', 'OnWalletConnected', connectedWalletString);
            })
            .catch((error) => {
                console.error("Error connecting to wallet:", error);
                window.unityInstanceRef.SendMessage('TonConnectHandler', 'OnWalletConnected', 'Error');
            });
    },
    
    jsBuyByStars: function () {
        window.Telegram.WebApp.openInvoice("https://t.me/$1K0N2ITWSUkaDQAAdtLZeXeXPfU");
    },
    
    jsSendTransaction: async function () {
        const transaction = {
            validUntil: Math.floor(Date.now() / 1000) + 60,
            messages: [
                {
                    address: 'UQCrooVhxL4NZYMjkZzq55qLjzaDP9wjP98KgdUEuSVxfroE',
                    amount: "20000000"
                }
            ]
        };

        try {
            const result = await tonConnectUI.sendTransaction(transaction);
            window.unityInstanceRef.SendMessage('TonConnectHandler', 'OnSend', 'Success');
        } catch (e) {
            window.unityInstanceRef.SendMessage('TonConnectHandler', 'OnSend', 'Error');
            console.error(e);
        }
    }
});
