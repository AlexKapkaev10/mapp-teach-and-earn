using System;
using Project.Scripts.Connect;

namespace Project.Scripts.Mining
{
    public interface IMainModel
    {
        void Claim(Action<bool, float> onClaim);
        void Buy();
        void UpgradeForStars();
        void ConnectWallet();
        void DisconnectWallet();
        void OnTransactionSend();
    }
    
    public class MainModel : IMainModel
    {
        private readonly ITelegramConnectService _connectService;

        public MainModel(ITelegramConnectService connectService)
        {
            _connectService = connectService;
        }
        
        public void Claim(Action<bool, float> onClaim)
        {
            _connectService.Claim(onClaim);
        }

        public void Buy()
        {
            _connectService.Buy();
        }

        public void UpgradeForStars()
        {
            _connectService.UpgradeForStars();
        }

        public void ConnectWallet()
        {
            _connectService.ConnectWallet();
        }

        public void DisconnectWallet()
        {
            _connectService.DisconnectWallet();
        }

        public void OnTransactionSend()
        {
            _connectService.OnTransactionSend();
        }
    }
}