using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Code.Configs.Views;
using Project.Scripts;
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
        void ClearLogsTimer();
    }
    
    public class MainPresenter : IMainPresenter
    {
        private readonly MainPresenterConfig _config;
        
        private readonly IMainModel _model;
        private readonly ITransactionHandler _transactionHandler;
        private IMainView _view;
        private CancellationTokenSource _cts;
        
        private const string k_onSuccess = "Success";
        private const string k_onError = "Error";
        
        public bool CanClaim { get; private set; } = true;

        [Inject]
        public MainPresenter(MainPresenterConfig config, IMainModel model, ITransactionHandler transactionHandler)
        {
            _config = config;
            _model = model;
            _transactionHandler = transactionHandler;
        }

        public void Init(IMainView view)
        {
            _transactionHandler.TransactionSend += OnTransactionSend;
            
            _view = view;
            _model.SetInit();
            _view.UpdateScore($"{_model.Score:F} POI");
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

        private void OnClaim(float claimValue)
        {
            _view.UpdateScore($"{_model.Score:F} POI");
            _view.UpdateLog($"Claimed {claimValue:F} poi", true);
            
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
            _view.UpdateScore(_model.Score.ToString("F"));
        }

        public void Dispose()
        {
            _transactionHandler.TransactionSend -= OnTransactionSend;
        }

        public void ClearLogsTimer()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            _cts = new CancellationTokenSource();
            ClearLogsAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid ClearLogsAsync(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(_config.ClearLogsDelay), token);
            
            _view.ClearLogs();
            _cts = null;
        }
    }
}