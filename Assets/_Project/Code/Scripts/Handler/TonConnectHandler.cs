using System;
using UnityEngine;

namespace Project.Scripts
{
    public interface ITransactionHandler
    {
        event Action<string> TransactionSend;
        event Action<string, bool> WalletConnect;
        void OnSend(string message);
    }
    
    public class TonConnectHandler : MonoBehaviour, ITransactionHandler
    {
        public event Action<string> TransactionSend;
        public event Action<string, bool> WalletConnect;

        private void OnDestroy()
        {
            TransactionSend = null;
        }

        public void OnSend(string message)
        {
            TransactionSend?.Invoke(message);
        }

        public void OnWalletConnected(string jsonMassage)
        {
            WalletConnect?.Invoke(jsonMassage, true);
        }
        
        public void OnWalletDisconnected()
        {
            WalletConnect?.Invoke(null, false);
        }
    }
}