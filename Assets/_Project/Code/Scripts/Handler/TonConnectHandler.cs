using System;
using Project.Infrastructure.Extensions;
using UnityEngine;

namespace Project.Scripts
{
    public interface ITransactionHandler
    {
        event Action<string> TransactionSend;
        void OnSend(string message);
    }
    
    public class TonConnectHandler : MonoBehaviour, ITransactionHandler
    {
        public event Action<string> TransactionSend;

        private void OnDestroy()
        {
            TransactionSend = null;
        }

        public void OnSend(string message)
        {
            TransactionSend?.Invoke(message);
        }

        public void OnWalletConnected(string jsonMassage)
        {
            Debug.Log(jsonMassage);
        }

        public void OnReceiveInitData(string initData)
        {
            
        }
        
        public void OnLaunchDataReceived(string jsonData)
        {
            Debug.Log(jsonData);
            
            if (jsonData != "Error")
            {
                TelegramData telegramData = JsonUtility.FromJson<TelegramData>(jsonData);
                string formattedAuthDate = ConvertTimestampToDate(telegramData.auth_date);
                
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

    [Serializable]
    public class TelegramData
    {
        public User user;
        public string chat_instance;
        public string chat_type;
        public long auth_date;
        public string signature;
        public string hash;
    }

    [Serializable]
    public class User
    {
        public long id;
        public string first_name;
        public string last_name;
        public string username;
        public string language_code;
        public bool is_premium;
        public bool allows_write_to_pm;
        public string photo_url;
    }
}