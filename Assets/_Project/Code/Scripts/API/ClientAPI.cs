using System;
using System.Collections;
using Project.Scripts.Bank;
using Project.Scripts.Tools;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace Project.Code.Scripts.API
{
    public interface IClientAPI
    {
        void RandomClaim(Action<bool, float> callBack);
        void TransactionSend();
    }
    
    public class ClientAPI : IClientAPI
    {
        private ICoroutineStarter _coroutineStarter;
        private IBank _bank;

        private const string _url = "https://jsonplaceholder.typicode.com/posts";

        [Inject]
        private void Construct(ICoroutineStarter coroutineStarter, IBank bank)
        {
            _coroutineStarter = coroutineStarter;
            _bank = bank;
        }

        public void RandomClaim(Action<bool, float> callBack)
        {
            _coroutineStarter.Starter.StartCoroutine(ClaimPointsAsync(callBack, _url));
        }

        public void TransactionSend()
        {
            _bank.SetPoints(100);
        }

        private IEnumerator ClaimPointsAsync(Action<bool, float> callBack, string url)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            float randomPoints = default;
            bool isSuccess = default;
            
            yield return request.SendWebRequest();
            
            isSuccess = request.result == UnityWebRequest.Result.Success;

            if (isSuccess)
            {
                Debug.Log("Response API: " + request.downloadHandler.text);
                randomPoints = _bank.ClaimPoints();
                _bank.SetPoints(randomPoints);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
            
            callBack?.Invoke(isSuccess, randomPoints);
        }
    }
}