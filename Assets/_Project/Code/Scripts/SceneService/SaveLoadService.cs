using UnityEngine;

namespace Project.Scripts.Architecture
{
    public interface ISaveLoadService
    {
        float GetMinClaim();
        float GetMaxClaim();
        float GetPoints();
        void SavePoints(float scoreValue);
        int GetCoins();
        void SaveCoins(int count);
        int GetLocaleID();
    }
    
    public class SaveLoadService : ISaveLoadService
    {
        private const string k_saveScoreKey = "saveScore";
        private const string k_saveGameCoinKey = "saveGameCoinCount";
        private const string k_saveMinClaimKey = "saveMinClaim";
        private const string k_saveMaxClaimKey = "saveMaxClaim";
        private const string k_saveLocaleIDKey = "saveLocaleID";

        public void SavePoints(float scoreValue)
        {
            PlayerPrefs.SetFloat(k_saveScoreKey, scoreValue);
        }

        public void SaveCoins(int count)
        {
            PlayerPrefs.SetInt(k_saveGameCoinKey, count);
        }

        public float GetPoints()
        {
            return PlayerPrefs.GetFloat(k_saveScoreKey, 0f);
        }

        public int GetCoins()
        {
            return PlayerPrefs.GetInt(k_saveGameCoinKey, 0);
        }

        public float GetMinClaim()
        {
            return PlayerPrefs.GetFloat(k_saveMinClaimKey, 0.1f);
        }

        public float GetMaxClaim()
        {
            return PlayerPrefs.GetFloat(k_saveMaxClaimKey, 0.5f);
        }
        
        public int GetLocaleID()
        {
            return PlayerPrefs.GetInt(k_saveLocaleIDKey, 0);
        }
    }
}