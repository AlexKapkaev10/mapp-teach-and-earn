mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
    console.log("Hello, world!");
    console.log(window.Telegram.WebApp.openInvoice);
  },
/*
  Init: function(){
    const script = document.createElement('script');
    script.src = 'https://telegram.org/js/telegram-web-app.js';
    document.appendChild(script);
    script.onload = () => {
      console.log('openInvoice', window.Telegram.WebApp.openInvoice);
    };

    script.onerror = () => {
      console.error('Failed to load Mini Apps SDK.');
    };
  }
 */ 
});