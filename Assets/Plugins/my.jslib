mergeInto(LibraryManager.library, {

  Hello: function () {
    window.Telegram.WebApp.openInvoice("https://t.me/$1K0N2ITWSUkaDQAAdtLZeXeXPfU");
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