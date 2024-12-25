using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class TimerItem : MonoBehaviour
    {
        [SerializeField] private string _mpcName = "mpc 1x";
        
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private TMP_Text _textRecCount;

        private void Awake()
        {
            SetHeader();
            UpdateTimer();
            SetRecCount("0");
        }

        public void SetHeader(in string textHeader = null)
        {
            _textHeader.SetText(textHeader);
        }

        public void UpdateTimer(in string textTimer = null)
        {
            if (textTimer == null)
            {
                _textTimer.SetText(_mpcName);
                return;
            }
            
            _textTimer.SetText(textTimer);
        }

        public void SetRecCount(in string textRecCount = null)
        {
            _textRecCount.SetText(textRecCount);
        }
    }
}