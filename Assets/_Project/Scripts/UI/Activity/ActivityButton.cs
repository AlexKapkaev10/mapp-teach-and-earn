using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Activity
{
    public interface IActivityButton
    {
        event Action<ViewStateType> Click;
        void SetData(ActivityButtonData data);
    }
    
    public class ActivityButton : MonoBehaviour, IActivityButton
    {
        public event Action<ViewStateType> Click;

        [SerializeField] private Button _button;
        [SerializeField] private Image _imageIcon;
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private TMP_Text _textDescription;

        private ViewStateType _switchToType;

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        public void SetData(ActivityButtonData data)
        {
            _switchToType = data.SwitchToStateType;
            _button.interactable = data.SwitchToStateType != ViewStateType.None;
            if (data.SwitchToStateType == ViewStateType.None)
                return;
            
            _textHeader.SetText(data.HeaderLocalizationKey);
            _textDescription.SetText(data.DescriptionLocalizationKey);
            _imageIcon.sprite = data.SpriteIcon;
        }

        private void OnClick()
        {
            Click?.Invoke(_switchToType);
        }
    }
}