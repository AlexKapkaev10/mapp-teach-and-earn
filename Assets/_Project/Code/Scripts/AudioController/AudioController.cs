using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Project.Scripts.Audio
{
    public interface IAudioController
    {
        void SetFxClip(in AudioMode mode);
        void SetAmbientClip(in AudioMode clip);
        void PlayFXClip();
        void PlayAmbientClip();
    }
    
    public sealed class AudioController : MonoBehaviour, IAudioController
    {
        [SerializeField] private AudioSource _audioSourceFX;
        [SerializeField] private AudioSource _audioSourceAmbient;
        
        private AudioControllerConfig _config;

        [Inject]
        private void Construct(AudioControllerConfig config)
        {
            _config = config;
            _config.InitConfig();
        }
        
        public void SetFxClip(in AudioMode mode)
        {
            _audioSourceFX.clip = _config.GetClipByMode(mode);
        }
        
        public void SetAmbientClip(in AudioMode mode)
        {
            var clip =  _config.GetClipByMode(mode);
            if (clip)
            {
                _audioSourceAmbient.clip = clip;
            }
            else
            {
                _audioSourceAmbient.Stop();
            }
        }

        public void PlayFXClip()
        {
            _audioSourceFX.Play();
        }
        
        public void PlayAmbientClip()
        {
            _audioSourceAmbient.volume = 0f;
            _audioSourceAmbient.loop = true;
            _audioSourceAmbient.Play();
            _audioSourceAmbient.DOFade(0.7f, 5f)
                .SetEase(Ease.Linear);
        }
    }
}