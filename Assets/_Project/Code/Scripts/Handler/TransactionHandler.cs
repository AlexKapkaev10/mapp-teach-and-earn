using System;
using UnityEngine;

namespace Project.Scripts
{
    public interface ITransactionHandler
    {
        event Action<string> TransactionSend;
        void OnSend(string message);
    }
    
    public class TransactionHandler : MonoBehaviour, ITransactionHandler
    {
        public event Action<string> TransactionSend;

        private void OnDestroy()
        {
            TransactionSend = null;
        }

        public void OnSend(string message)
        {
            TransactionSend?.Invoke(message);
        }
    }
}