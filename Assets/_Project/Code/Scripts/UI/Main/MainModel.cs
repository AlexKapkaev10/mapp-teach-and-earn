using System;
using System.Runtime.InteropServices;
using Project.Infrastructure.Extensions;
using Project.Scripts.API;
using UnityEngine;

namespace Project.Scripts.Module.Mining
{
    public interface IMainModel
    {
        void SetInit();
        void Claim(Action<bool, float> callBack);
        void UpgradeForCoins();
        void UpgradeForStars();
        void Buy();
        void OnTransactionSend();
        void ConnectWallet();
    }
    
    public class MainModel : IMainModel
    {
        [DllImport("__Internal")]
        private static extern void jsInit();

        [DllImport("__Internal")]
        private static extern void jsConnectWallet();

        [DllImport("__Internal")]
        private static extern void jsBuyByStars();

        [DllImport("__Internal")]
        private static extern void jsSendTransaction();
        
        [DllImport("__Internal")]
        private static extern void jsShareLink(string link, string text);

        private readonly IClientAPI _clientAPI;
        private bool isInit;

        public MainModel(IClientAPI clientAPI)
        {
            _clientAPI = clientAPI;
        }

        public void SetInit()
        {
            if (isInit)
            {
                return;
            }

#if !UNITY_EDITOR
            jsInit();
#endif
            isInit = true;
        }

        public void ConnectWallet()
        {
#if !UNITY_EDITOR
            jsConnectWallet();
#else
            this.Log("Editor Connect Wallet");
#endif
        }

        public void UpgradeForStars()
        {
#if !UNITY_EDITOR
            jsBuyByStars();
#else
            this.Log("Editor Upgrade For Stars");
#endif
        }

        public void Buy()
        {
#if !UNITY_EDITOR
            jsShareLink("https://google.com", "Hello");
#else
            this.Log("Editor Buy");
#endif
        }

        public void Claim(Action<bool, float> callBack)
        {
            _clientAPI.RandomClaimAsync(callBack);
        }

        public void UpgradeForCoins()
        {
            this.Log("Editor Upgrade For Coins");
        }

        public void OnTransactionSend()
        {
            _clientAPI.ConfirmTransaction();
        }

        public void OpenUrl()
        {
            string url = "https://t.me/alexoneDevelop";
            Application.OpenURL(url);
        }
    }
}