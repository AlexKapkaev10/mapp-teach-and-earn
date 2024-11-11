using System;
using VContainer.Unity;

namespace Project.Scripts.Bank
{
    public interface IBankPresenter : IInitializable
    {
        event Action<string> GameCoinValueChange;
        string GetGameCoinCount();
        void AddGameCoin(int count);
    }
    
    public class BankPresenter : IBankPresenter
    {
        public event Action<string> GameCoinValueChange;
        
        private readonly IBankModel _model;

        public BankPresenter(IBankModel model)
        {
            _model = model;
        }

        public string GetGameCoinCount()
        {
            return _model.GameCoinCount.ToString();
        }

        public void Initialize()
        {
            _model.GameCoinValueChange += OnGameCoinValueChange;
        }
        
        public void AddGameCoin(int count)
        {
            _model.SetGameCoins(count);
        }

        private void OnGameCoinValueChange(int value)
        {
            GameCoinValueChange?.Invoke(value.ToString());
        }
    }
}