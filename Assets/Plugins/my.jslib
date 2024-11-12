let tonConnectUI;

mergeInto(LibraryManager.library, {


    Init: function (){
    tonConnectUI = new TON_CONNECT_UI.TonConnectUI({
      manifestUrl: 'https://tops-bullfrog-painfully.ngrok-free.app/tonconnect-manifest.json',
      buttonRootId: 'connect-button'
  });
  },

    BuyForStars: function () {
    window.Telegram.WebApp.openInvoice("https://t.me/$1K0N2ITWSUkaDQAAdtLZeXeXPfU");
  },

    Send: async function () {
    const transaction = {
      validUntil: Math.floor(Date.now() / 1000) + 60, // 60 sec
      messages: [
          {
              address: 'UQCrooVhxL4NZYMjkZzq55qLjzaDP9wjP98KgdUEuSVxfroE',
              amount: "20000000",
           // stateInit: "base64bocblahblahblah==" // just for instance. Replace with your transaction initState or remove
          }
      ]
  }
  
  try {
      const result = await tonConnectUI.sendTransaction(transaction);
      // const someTxData = await myAppExplorerService.getTransaction(result.boc);
      window.unityInstanceRef.SendMessage('TransactionHandler', 'OnSend', 'Success');
  } catch (e) {
      window.unityInstanceRef.SendMessage('TransactionHandler', 'OnSend', 'Error');
      console.error(e);
  }
  }
});