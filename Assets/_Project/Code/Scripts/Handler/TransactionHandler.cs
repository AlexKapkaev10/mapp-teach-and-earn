using System;
using Project.Code.Scripts.API;
using TMPro;
using UnityEngine;
using VContainer;

namespace Project.Scripts
{
    public class TransactionHandler : MonoBehaviour
    {
        public event Action TransactionSend;
        
        [SerializeField] private TMP_Text _textSendMessage;

        private const string k_onSuccess = "Success";
        private const string k_onError = "Error";

        [Inject]
        private void Configure(ITestAPI testAPI)
        {
            Debug.Log(testAPI);
        }
        
        public void OnSend(string message)
        {
            _textSendMessage.SetText(message);
            
            if (message == k_onSuccess)
            {
                TransactionSend?.Invoke();
            }
        }
    }
}