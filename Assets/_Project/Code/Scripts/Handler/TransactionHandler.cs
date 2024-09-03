using System;
using Project.Code.Scripts.API;
using UnityEngine;
using VContainer;

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

        [Inject]
        private void Configure(ITestAPI testAPI)
        {
            Debug.Log(testAPI);
        }

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