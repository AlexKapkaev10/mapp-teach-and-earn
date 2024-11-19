using System;
using System.Collections;
using Project.Scripts.Architecture;
using Project.Scripts.Tools;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Project.Code.Scripts.API
{
    public interface IClientAPI : IInitializable
    {
        float GetScore();
        void RandomClaim(Action<float> callBack);
        void TransactionSend();
    }
    
    public class ClientAPI : IClientAPI
    {
        private ICoroutineStarter _coroutineStarter;
        private ISaveLoadService _saveLoadService;

        private float _minClaim = 0.1f;
        private float _maxClaim = 0.5f;
        
        private float _score;
        
        private const string _url = "https://jsonplaceholder.typicode.com/posts";

        [Inject]
        private void Construct(ICoroutineStarter coroutineStarter, ISaveLoadService saveLoadService)
        {
            _coroutineStarter = coroutineStarter;
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            _score = _saveLoadService.GetPoints();
        }

        public float GetScore()
        {
            return _saveLoadService.GetPoints();
        }

        public void RandomClaim(Action<float> callBack)
        {
            _coroutineStarter.Starter.StartCoroutine(GetRandomClaimAsync(callBack, _url));
        }

        public void TransactionSend()
        {
            AddScore(100f);
        }

        private void AddScore(float addValue)
        {
            _score += addValue;
            _saveLoadService.SavePoints(_score);
        }

        private IEnumerator GetRandomClaimAsync(Action<float> callBack, string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
                else
                {
                    Debug.Log("Response API: " + request.downloadHandler.text);
                }
                
                var random = Random.Range(_saveLoadService.GetMinClaim(), _saveLoadService.GetMaxClaim());
                AddScore(random);
                callBack?.Invoke(random);
            }
        }
        
        private IEnumerator GetRequestAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
                else
                {
                    Debug.Log("Response API: " + request.downloadHandler.text);
                }
            }
        }
        
    }
}