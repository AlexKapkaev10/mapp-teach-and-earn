using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public interface ISwitchViewItem
    {
        event Action<ISwitchViewItem> SwitchView;
        ViewStateType Type { get; }
        void SetActive(bool isActive);
    }
    
    public class SwitchViewItem : MonoBehaviour, ISwitchViewItem
    {
        public event Action<ISwitchViewItem> SwitchView;
        
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ViewStateType _stateType;

        public ViewStateType Type => _stateType;

        public void SetActive(bool isActive)
        {
            var alphaValue = isActive ? 1f : 0f;

            _canvasGroup.alpha = alphaValue;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            SwitchView?.Invoke(this);
        }
    }
}