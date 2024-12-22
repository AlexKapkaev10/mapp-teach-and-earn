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
        void PlayClip();
    }
    
    public class DrumMachineItem : MonoBehaviour, IDrumMachineItem
    {
        [field: SerializeField] public int Index { get; private set; }

        [SerializeField] private Image _imageIndicator;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Color _colorIndicatorDefault;

        [SerializeField] private KeyCode _keyCodePress;
        [SerializeField] private Color _colorPress = Color.cyan;
        public event Action<int> Clicked;

        private void Awake()
        {
            _audioSource.clip = _clip;
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keyCodePress))
            {
                PlayClip();
                SwitchColor(_colorPress);
            }

            if (Input.GetKeyUp(_keyCodePress))
            {
                SwitchColor(_colorIndicatorDefault);
            }
        }

        public void PlayClip()
        {
            _audioSource.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Clicked?.Invoke(Index);
            PlayClip();
            SwitchColor(_colorPress);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SwitchColor(_colorIndicatorDefault);
        }

        private void SwitchColor(in Color color)
        {
            _imageIndicator.color = color;
        }
    }
}