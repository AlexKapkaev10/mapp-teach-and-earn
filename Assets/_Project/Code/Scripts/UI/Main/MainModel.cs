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
        void TransactionSend();
    }
    
    public class MainModel : IMainModel
    {
        [DllImport("__Internal")]
        private static extern void BuyForStars();
    
        [DllImport("__Internal")]
        private static extern void Init();
    
        [DllImport("__Internal")]
        private static extern void Send();
        
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
            Init();
#endif
            isInit = true;
        }

        public void Claim(Action<bool, float> callBack)
        {
            _clientAPI.RandomClaimAsync(callBack);
        }

        public void UpgradeForStars()
        {
#if !UNITY_EDITOR
            BuyForStars();
#else
            this.Log("Editor Upgrade For Stars");
#endif
        }
    
        public void Buy()
        {
#if !UNITY_EDITOR
            Send();
#else
            this.Log("Editor Buy");
#endif
        }

        public void UpgradeForCoins()
        {
            this.Log("Editor Upgrade For Coins");
        }

        public void TransactionSend()
        {
            _clientAPI.TransactionSend();
        }
        
        public void OpenUrl()
        {
            string url = "https://t.me/alexoneDevelop";
            Application.OpenURL(url);
        }
    }
}