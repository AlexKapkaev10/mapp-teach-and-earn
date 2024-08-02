
using UnityEngine;

namespace Project.Code.Scripts.API
{
    public interface ITestAPI
    {
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

        public float GetScore()
        {
            return PlayerPrefs.GetFloat(k_saveScoreKey, 0);
        }

        public float GetRandomClaim()
        {
            var random = Random.Range(_minClaim, _maxClaim);
            SetScore(random);
            return _score;
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
    }
}