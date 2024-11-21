using System;
using Project.Scripts.Architecture;
using VContainer;
using Random = UnityEngine.Random;

namespace Project.Scripts.Bank
{
    public interface IBank
    {
        event Action<int> CoinValueChanged;
        float ClaimPoints();
        int GetCoins();
        void SetCoins(int coinsValue);
        float GetPoints();
        void SetPoints(float pointsValue);
    }

    public class Bank : IBank
    {
        private readonly ISaveLoadService _saveLoadService;
        public event Action<int> CoinValueChanged;

        [Inject]
        public Bank(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }
        
        public int GetCoins()
        {
            return _saveLoadService.GetCoins();
        }

        public void SetCoins(int coinsValue)
        {
            var gameCoins = _saveLoadService.GetCoins() + coinsValue;
            
            _saveLoadService.SaveCoins(gameCoins);
            CoinValueChanged?.Invoke(gameCoins);
        }

        public void SetPoints(float pointsValue)
        {
            var points = _saveLoadService.GetPoints() + pointsValue;
            _saveLoadService.SavePoints(points);
        }

        public float ClaimPoints()
        {
            var randomPoints = Random.Range(GetMinClaim(), GetMaxClaim());
            return randomPoints;
        }

        public float GetPoints()
        {
           return _saveLoadService.GetPoints();
        }

        private float GetMinClaim()
        {
            return _saveLoadService.GetMinClaim();
        }

        private float GetMaxClaim()
        {
            return _saveLoadService.GetMaxClaim();
        }
    }
}