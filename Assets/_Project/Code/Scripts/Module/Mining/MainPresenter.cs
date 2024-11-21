using System;
using Project.Code.Configs.Views;
using Project.Scripts;
using Project.Scripts.Bank;
using VContainer;

namespace Project.Code.Scripts.Module.Mining
{
    public interface IMainPresenter : IDisposable
    {
        bool CanClaim { get; }
        void Init(IMainView view);
        void ClaimClick();
        void Buy();
        void UpgradeRandom();
    }
    
    public class MainPresenter : IMainPresenter
    {
        private readonly MainPresenterConfig _config;
        
        private readonly IMainModel _model;
        private readonly ITransactionHandler _transactionHandler;
        private IMainView _view;
        private readonly IBank _bank;

        private const string k_onSuccess = "Success";
        private const string k_onError = "Error";
        
        public bool CanClaim { get; private set; } = true;

        [Inject]
        public MainPresenter(
            IMainModel model, 
            ITransactionHandler transactionHandler,
            IBank bank,
            MainPresenterConfig config)
        {
            _config = config;
            _model = model;
            _bank = bank;
            _transactionHandler = transactionHandler;
        }

        public void Init(IMainView view)
        {
            _transactionHandler.TransactionSend += OnTransactionSend;
            
            _view = view;
            _model.SetInit();
            _view.UpdateScore($"{_bank.GetPoints():F} POI");
        }

        private void OnTransactionSend(string message)
        {
            var isSuccess = message == k_onSuccess;
            
            if (isSuccess)
            {
                TransactionSend();
            }
            
            _view.UpdateLog(message, isSuccess);
        }

        public void ClaimClick()
        {
            _model.Claim(OnClaim);
            _view.UpdateClaimButton(false);
        }

        private void OnClaim(bool isSuccess, float claimPoints)
        {
            _view.UpdateScore($"{_bank.GetPoints():F} POI");
            _view.UpdateLog($"Claimed {claimPoints:F} poi", true);
            
            CanClaim = false;
        }

        public void Buy()
        {
            _model.Buy();
        }

        public void UpgradeRandom()
        {
            _model.UpgradeForCoins();
        }

        public void TransactionSend()
        {
            _model.TransactionSend();
            _view.UpdateScore(_bank.GetPoints().ToString("F"));
        }

        public void Dispose()
        {
            _transactionHandler.TransactionSend -= OnTransactionSend;
        }
    }
}