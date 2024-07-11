mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
    console.log("Hello, world!");
    console.log(window.Telegram.WebApp.openInvoice);
  },
});