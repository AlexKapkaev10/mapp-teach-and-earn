using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts.Audio
{
    public interface IDrumMachineItem : IPointerDownHandler, IPointerUpHandler
    {
        event Action<int> Clicked;
        int Index { get; }
        void SetIndex(int i);
        void PlayClip();
    }
    
    public class DrumMachineItem : MonoBehaviour, IDrumMachineItem
    {
        [field: SerializeField] public int Index { get; private set; }

        [SerializeField] private Image _imageIndicator;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private Color _colorIndicatorDefault;
        [SerializeField] private Color _colorPress = Color.cyan;

        public event Action<int> Clicked;
        
        private void Awake()
        {
            _audioSource.clip = _clip;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayAndInvoke();
            SwitchColor(_colorPress);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SwitchColor(_colorIndicatorDefault);
        }

        public void PlayClip()
        {
            _audioSource.Play();
        }

        public void SetIndex(int i)
        {
            Index = i;
        }

        private void PlayAndInvoke()
        {
            Clicked?.Invoke(Index);
            PlayClip();
        }

        private void SwitchColor(in Color color)
        {
            _imageIndicator.color = color;
        }
    }
}