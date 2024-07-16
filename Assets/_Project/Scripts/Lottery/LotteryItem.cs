using TMPro;
using UnityEngine;

namespace Project.Lottery
{
    public interface ILotteryItem
    {
        void SetHeader(string textHeader);
        string GetHeader();
        void Destroy();
    }
    
    public class LotteryItem : MonoBehaviour, ILotteryItem
    {
        [SerializeField] private TMP_Text _textHeader;
        public void SetHeader(string textHeader)
        {
            _textHeader.SetText(textHeader);
        }

        public string GetHeader()
        {
            return _textHeader.text;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}