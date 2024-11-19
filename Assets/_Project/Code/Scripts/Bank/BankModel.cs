using System;
using Project.Scripts.Architecture;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Bank
{
    public interface IBankModel : IInitializable
    {
        event Action<int> GameCoinValueChange;
        int GameCoinCount { get; }
        void SetGameCoins(int value);
    }

    public class BankModel : IBankModel
    {
        public event Action<int> GameCoinValueChange;

        private readonly ISaveLoadService _saveLoadService;
        private int _gameCoinCount;

        public int GameCoinCount => _gameCoinCount;

        [Inject]
        public BankModel(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            _gameCoinCount = _saveLoadService.GetCoinsCount();
        }

        public void SetGameCoins(int value)
        {
            _gameCoinCount += value;
            _saveLoadService.SaveCoinsCount(_gameCoinCount);
            GameCoinValueChange?.Invoke(_gameCoinCount);
        }
    }
}