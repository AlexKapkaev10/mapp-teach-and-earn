using UnityEngine;

namespace Project.Scripts.Architecture
{
    public interface ISaveLoadService
    {
        float GetMinClaim();
        float GetMaxClaim();
        float GetPoints();
        void SavePoints(float scoreValue);
        int GetCoinsCount();
        void SaveCoinsCount(int count);
    }
    
    public class SaveLoadService : ISaveLoadService
    {
        private const string k_saveScoreKey = "saveScore";
        private const string _saveGameCoinKey = "saveGameCoinCount";
        
        private const string _saveMinClaimKey = "saveMinClaim";
        private const string _saveMaxClaimKey = "saveMaxClaim";

        public void SavePoints(float scoreValue)
        {
            PlayerPrefs.SetFloat(k_saveScoreKey, scoreValue);
        }

        public float GetMinClaim()
        {
            return PlayerPrefs.GetFloat(_saveMinClaimKey, 0.1f);
        }

        public float GetMaxClaim()
        {
            return PlayerPrefs.GetFloat(_saveMaxClaimKey, 0.5f);
        }

        public float GetPoints()
        {
            return PlayerPrefs.GetFloat(k_saveScoreKey, 0f);
        }

        public void SaveCoinsCount(int count)
        {
            PlayerPrefs.SetInt(_saveGameCoinKey, count);
        }

        public int GetCoinsCount()
        {
            return PlayerPrefs.GetInt(_saveGameCoinKey, 0);
        }
    }
}