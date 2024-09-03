using System;
using _Project.Code.Configs.Views;
using Project.Scripts;

namespace Project.Code.Scripts.Module.Mining
{
    public interface IMainPresenter : IDisposable
    {
        void Init(IMainView view);
        void Claim();
        void Buy();
        void Upgrade();
    }
    
    public class MainPresenter : IMainPresenter
    {
        private readonly MainPresenterConfig _config;
        
        private readonly IMainModel _model;
        private readonly ITransactionHandler _transactionHandler;
        private IMainView _view;

        private const string k_onSuccess = "Success";
        private const string k_onError = "Error";
        
        public MainPresenter(MainPresenterConfig config, IMainModel model, ITransactionHandler transactionHandler)
        {
            _config = config;
            _model = model;
            _transactionHandler = transactionHandler;
            _transactionHandler.TransactionSend += OnTransactionSend;
        }

        public void Init(IMainView view)
        {
            _view = view;
            _view.UpdateScore(_model.Score.ToString("F"));
            _model.SetInit();
        }

        private void OnTransactionSend(string message)
        {
            if (message == k_onSuccess)
            {
                TransactionSend();
            }
            
            _view.UpdateLog(message);
        }

        public void Claim()
        {
            _model.Claim(out var value);
            _view.UpdateScore(value.ToString("F"));
        }

        public void Buy()
        {
            _model.Buy();
        }

        public void Upgrade()
        {
            _model.Upgrade();
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
    }
}