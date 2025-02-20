using System;
using Project.Configs.Views;
using Project.Scripts.Bank;
using VContainer;

namespace Project.Scripts.Mining
{
    public interface IMainPresenter : IDisposable
    {
        bool CanClaim { get; }
        void Init(IMainView view);
        void ClaimClick();
        void Buy();
        void UpgradeRandom();
        void ConnectWallet();
        void DisconnectWallet();
    }
    
    public class MainPresenter : IMainPresenter
    {
        private readonly MainPresenterConfig _config;
        
        private readonly IMainModel _model;
        private readonly ITransactionHandler _transactionHandler;
        private readonly IBank _bank;
        private IMainView _view;
        
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
            _view.UpdateScore($"{_bank.GetPoints():F} {_config.POI}");
        }

        private void OnTransactionSend(string message)
        {
            var isSuccess = message == _config.Success;
            
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
            _view.UpdateScore($"{_bank.GetPoints():F} {_config.POI}");
            _view.UpdateLog($"Claimed {claimPoints:F} {_config.POI}", true);
            
            CanClaim = false;
        }

        public void Buy()
        {
            _model.Buy();
        }

        public void UpgradeRandom()
        {
            _model.UpgradeForStars();
        }

        public void ConnectWallet()
        {
            _model.ConnectWallet();
        }

        public void DisconnectWallet()
        {
            _model.DisconnectWallet();
        }

        public void TransactionSend()
        {
            _model.OnTransactionSend();
            _view.UpdateScore(_bank.GetPoints().ToString("F"));
        }

        public void Dispose()
        {
            _transactionHandler.TransactionSend -= OnTransactionSend;
        }
    }
}