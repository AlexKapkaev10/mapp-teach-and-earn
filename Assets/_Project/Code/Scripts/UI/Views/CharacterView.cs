using System;
using Project.Scripts.Audio;
using Project.Scripts.Configs.Views;
using Project.Scripts.UI;
using UnityEngine;
using VContainer;

namespace Project.Scripts.GameCharacter
{
    public class CharacterView : View
    {
        [SerializeField] private CharacterViewConfig _config;
        [SerializeField] private RectTransform _characterParent;
        private IAudioController _audioController;
        private ICharacter _character;
        
        [Inject]
        private void Construct(IAudioController audioController)
        {
            _audioController = audioController;
            _audioController.SetFxClip(AudioMode.CharacterClick);
            _character = Instantiate(_config.CharacterPrefab, _characterParent);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _character.Click += OnClickCharacter;
        }

        private void OnDisable()
        {
            _character.Click -= OnClickCharacter;
        }

        private void OnClickCharacter()
        {
            _audioController.PlayFXClip();
        }

        private void Awake()
        {
            _audioController.SetAmbientClip(AudioMode.Home);
        }
    }
}