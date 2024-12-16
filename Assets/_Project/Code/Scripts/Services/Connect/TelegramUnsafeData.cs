using System;

namespace Project.Scripts.Connect
{
    [Serializable]
    public class TelegramUnsafeData
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