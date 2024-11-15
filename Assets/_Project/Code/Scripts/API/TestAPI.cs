
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Code.Scripts.API
{
    public interface ITestAPI
    {
        void Init();
        float GetScore();
        float GetRandomClaim();
        void TransactionSend();
    }
    
    public class TestAPI : ITestAPI
    {
        private float _minClaim = 0.1f;
        private float _maxClaim = 0.5f;

        private float _score;

        private const string k_saveScoreKey = "saveScore";

        public void Init()
        {
            _score = PlayerPrefs.GetFloat(k_saveScoreKey, 0);
        }

        public float GetScore()
        {
            return PlayerPrefs.GetFloat(k_saveScoreKey, 0);
        }

        public float GetRandomClaim()
        {
            var random = Random.Range(_minClaim, _maxClaim);
            
            SetScore(random);
            return random;
        }

        public void TransactionSend()
        {
            SetScore(100f);
        }

        private void SetScore(float addValue)
        {
            _score += addValue;
            PlayerPrefs.SetFloat(k_saveScoreKey, _score);
        }


        private async UniTaskVoid SetRandomClaimAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            
            
        }
    }
}