using Project.Infrastructure.Extensions;
using Project.Scripts.Connect;
using UnityEngine;
using VContainer;

namespace Project.Scripts
{
    public class TelegramConnectHandler : MonoBehaviour
    {
        private ITelegramConnectService _connectService;

        [Inject]
        private void Construct(ITelegramConnectService connectService)
        {
            _connectService = connectService;
        }

        private void Start()
        {
            _connectService.Init();
        }
        
        public void OnReceiveInitData(string initData)
        {
            this.Log(initData);
            _connectService.OnInitDataResponse(initData);
        }

        public void OnLaunchDataReceived(string message)
        {
            this.Log(message);
        }

        public void OnWalletDisconnected()
        {
            Debug.Log("Wallet Disconnect");
        }
    }
}