using System;
using System.Runtime.InteropServices;
using Project.Code.Scripts.API;
using UnityEngine;

namespace Project.Code.Scripts.Module.Mining
{
    public interface IMainModel
    {
        float Score { get; }
        void SetInit();
        void Claim(Action<float> callBack);
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
        private float _score;
        private bool isInit;

        public float Score => _clientAPI.GetScore();
        
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

        public void Claim(Action<float> callBack)
        {
            _clientAPI.RandomClaim(callBack);
        }

        public void UpgradeForStars()
        {
#if !UNITY_EDITOR
            BuyForStars();
#else
            Debug.Log("Editor Upgrade For Stars");
#endif
        }
    
        public void Buy()
        {
#if !UNITY_EDITOR
            Send();
#else
            Debug.Log("Editor Buy");
#endif
        }

        public void UpgradeForCoins()
        {
            Debug.Log("Editor Upgrade For Coins");
        }

        public void TransactionSend()
        {
            _clientAPI.TransactionSend();
        }
    }
}