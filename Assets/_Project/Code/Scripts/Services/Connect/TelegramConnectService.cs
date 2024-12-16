using System;
using Project.Scripts.API;
using Project.Scripts.Loader;

namespace Project.Scripts.Connect
{
    public interface ITelegramConnectService
    {
        void OnInitDataResponse(string initData);
        void Init();
        void Claim(Action<bool, float> callBack);
        void UpgradeForCoins();
        void UpgradeForStars();
        void Buy();
        void OnTransactionSend();
        void ConnectWallet();
        void ShareLink(string link, string header);
    }
    
    public class TelegramConnectService : ITelegramConnectService
    {
        private readonly ITelegramConnectModel _model;

        public TelegramConnectService(
            IClientAPI clientAPI, 
            ILoaderService loaderService, 
            TelegramConnectServiceConfig config)
        {
            _model = new TelegramConnectModel(clientAPI, loaderService, config);
        }

        public void OnInitDataResponse(string initData)
        {
            _model.OnInitDataResponse(initData);
        }

        public void Init()
        {
            _model.Init();
        }

        public void Claim(Action<bool, float> callBack)
        {
            _model.Claim(callBack);
        }

        public void UpgradeForCoins()
        {
            _model.UpgradeForCoins();
        }

        public void UpgradeForStars()
        {
            _model.UpgradeForStars();
        }

        public void Buy()
        {
            _model.Buy();
        }

        public void OnTransactionSend()
        {
            _model.OnTransactionSend();
        }

        public void ConnectWallet()
        {
            _model.ConnectWallet();
        }

        public void ShareLink(string link, string header)
        {
            _model.ShareLink(link, header);
        }
    }
}