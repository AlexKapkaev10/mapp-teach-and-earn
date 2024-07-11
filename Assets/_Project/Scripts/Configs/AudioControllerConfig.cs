using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Audio
{
    [CreateAssetMenu(fileName = nameof(AudioControllerConfig), menuName = "Configs/Audio/AudioControllerConfig")]
    public class AudioControllerConfig : ScriptableObject
    {
        [field: SerializeField] public AudioClip ClickCoinClip { get; private set; }
        [field: SerializeField] public AudioClip ClickerAmbientClip { get; private set; }

        [SerializeField] private AudioModeData[] _audioClipsData;
        
        private Dictionary<AudioMode, AudioClip> _audioClipDictionary;

        public void InitConfig()
        {
            _audioClipDictionary = new Dictionary<AudioMode, AudioClip>();
            
            foreach (var data in _audioClipsData)
            {
                _audioClipDictionary.Add(data.AudioMode, data.AudioClip);
            }
        }

        public AudioClip GetClipByMode(AudioMode mode)
        {
            return _audioClipDictionary.TryGetValue(mode, out AudioClip clip) ? clip : null;
        }
    }

    [Serializable]
    public struct AudioModeData
    {
        public AudioMode AudioMode;
        public AudioClip AudioClip;
    }

    public enum AudioMode
    {
        Home,
        ClickCoin,
        ClickerAmbient,
        CharacterClick
    }
}