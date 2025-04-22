using System;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using Project.Infrastructure.Extensions;
using Project.Scripts.API;
using Project.Scripts.Loader;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Connect
{
    public interface ITelegramConnectModel
    {
        void Init();
        void Claim(Action<bool, float> callBack);
        void UpgradeForCoins();
        void UpgradeForStars();
        void Buy();
        void OnTransactionSend();
        void ConnectWallet();
        void ShareLink(string link, string header);
        void OnInitDataResponse(string initData);
        void DisconnectWallet();
    }
    
    public class TelegramConnectModel : ITelegramConnectModel
    {
        [DllImport("__Internal")]
        private static extern void jsInit(string manifestUrl);

        [DllImport("__Internal")]
        private static extern void jsConnectWallet();

        [DllImport("__Internal")]
        private static extern void jsBuyByStars();

        [DllImport("__Internal")]
        private static extern void jsSendTransaction();
        
        [DllImport("__Internal")]
        private static extern void jsShareLink(string link, string text);
        
        [DllImport("__Internal")]
        private static extern void jsDisconnectWallet();
        
        private bool _isInit;
        private readonly IClientAPI _clientAPI;
        private readonly ILoaderService _loaderService;
        private readonly TelegramConnectServiceConfig _config;

        public TelegramConnectModel(
            IClientAPI clientAPI, 
            ILoaderService loaderService, 
            TelegramConnectServiceConfig config)
        {
            _clientAPI = clientAPI;
            _loaderService = loaderService;
            _config = config;
        }

        public void Init()
        {
            if (_isInit)
            {
                return;
            }

#if !UNITY_EDITOR
            jsInit(_config.ManifestUrl);
#else
            this.Log("Editor Init");
            OnInitDataResponse(_config.EditorInitData);
#endif
            _isInit = true;
        }

        public void ConnectWallet()
        {
#if !UNITY_EDITOR
            jsConnectWallet();
#else
            this.Log("Editor Connect Wallet");
#endif
        }
        
        public void Claim(Action<bool, float> callBack)
        {
            _clientAPI.RandomClaimAsync(callBack);
        }

        public void UpgradeForCoins()
        {
            this.Log("Editor Upgrade For Coins");
        }

        public void OnTransactionSend()
        {
            _clientAPI.ConfirmTransaction();
        }

        public void UpgradeForStars()
        {
#if !UNITY_EDITOR
            jsBuyByStars();
#else
            this.Log("Editor Upgrade For Stars");
#endif
        }

        public void Buy()
        {
#if !UNITY_EDITOR
            jsSendTransaction();
#else
            this.Log("Editor Buy");
#endif
        }

        public void ShareLink(string link, string header)
        {
#if !UNITY_EDITOR
            jsShareLink(link, header);
#else
            this.Log("Editor Buy");
#endif
        }

        public async void OnInitDataResponse(string initData)
        {
            try
            {
                var response = await SendInitDataAsync(initData);
                Debug.Log(response);
                _loaderService.StartLoadResources();
            }
            catch (Exception e)
            {
                this.Log(e.Message);
            }
        }

        public void DisconnectWallet()
        {
#if !UNITY_EDITOR
            jsDisconnectWallet();
#else
            this.Log("Editor Disconnect Wallet");
#endif
        }
        
        public async UniTask<bool> ValidateInitDataAsync(string initData)
        {
            string encodedInitData = Uri.EscapeDataString(initData);
            string url = $"https://api.telegram.org/bot{_config.BotToken}/validateWebAppInitData?initData={encodedInitData}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("InitData is valid: " + request.downloadHandler.text);
                    return true;
                }
                else
                {
                    Debug.LogError("Validation failed: " + request.error);
                    return false;
                }
            }
        }
        
        private async UniTask<string> SendInitDataAsync(string initData)
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes($"{{\"{_config.DataKey}\": \"{initData}\"}}");
            
            try
            {
                using (UnityWebRequest request = new UnityWebRequest(_config.ServerUrl, "POST"))
                {
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader(_config.ContentTypeKey, _config.ApplicationKey);
                
                    await request.SendWebRequest();

                    return request.result == UnityWebRequest.Result.Success ? request.downloadHandler.text : null;
                }
            }
            catch (Exception e)
            {
                this.Log($"Validation fail: {e.Message}");
            }

            return null;
        }
        
        public void OnLaunchDataReceived(string jsonData)
        {
            Debug.Log(jsonData);
            
            if (jsonData != "Error")
            {
                TelegramUnsafeData telegramUnsafeData = JsonUtility.FromJson<TelegramUnsafeData>(jsonData);
                string formattedAuthDate = ConvertTimestampToDate(telegramUnsafeData.auth_date);
                
                /*Debug.Log($"Parsed Auth Date: {formattedAuthDate}");
                Debug.Log($"User Info: {telegramData.user.first_name} {telegramData.user.last_name} ({telegramData.user.username})");
                Debug.Log($"User Info: {telegramData.user.id})");
                Debug.Log($"Chat Type: {telegramData.chat_type}");*/
            }
            else
            {
                this.LogError("Failed to retrieve launch data");
            }
        }

        private string ConvertTimestampToDate(long timestamp)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            DateTime dateTime = dateTimeOffset.LocalDateTime;
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}