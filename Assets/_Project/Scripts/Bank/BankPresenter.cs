using System;

namespace _Project.Scripts.Bank
{
    public interface IBankPresenter
    {
        event Action<string> GameCoinValueChange;
        string GetGameCoinCount();
        void InitModel();
        void AddGameCoin(int count);
    }
    
    public class BankPresenter : IBankPresenter
    {
        public event Action<string> GameCoinValueChange;
        
        private readonly IBankModel _model;

        public BankPresenter(IBankModel model)
        {
            _model = model;
            _model.GameCoinValueChange += OnGameCoinValueChange;
        }

        public string GetGameCoinCount()
        {
            return _model.GameCoinCount.ToString();
        }

        public void InitModel()
        {
            _model.Init();
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