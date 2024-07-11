using UnityEngine;

namespace _Project.Scripts.SaveLoad
{
    public interface ISaveLoadService
    {
        int LoadCoinsCount();
        void SaveCoinsCount(int count);
    }
    
    public class SaveLoadServiceSimple : ISaveLoadService
    {
        private const string _saveGameCoinKey = "saveGameCoinCount";

        public int LoadCoinsCount()
        {
            return PlayerPrefs.GetInt(_saveGameCoinKey, 0);
        }

        public void SaveCoinsCount(int count)
        {
            PlayerPrefs.SetInt(_saveGameCoinKey, count);
        }
    }
}