using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts.UI.Items
{
    public class DrumMachineItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _audioSource.clip = _clip;
        }

        private void OnEnable()
        {
            //_button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            //_button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _audioSource.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _audioSource.Play();
        }
    }
}