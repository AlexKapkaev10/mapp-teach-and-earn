using UnityEngine;

namespace Project.Scripts
{
    public class Metronome : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _bpm = 120;
        
        private float beatInterval;
        private float nextBeatTime;

        private void OnEnable()
        {
            UpdateBeatInterval();
            nextBeatTime = Time.time;
        }

        private void OnDisable()
        {
            nextBeatTime = default;
        }

        private void Update()
        {
            if (Time.time >= nextBeatTime)
            {
                PlayTick();
                nextBeatTime += beatInterval;
            }
        }

        public void SetBPM(int newBPM)
        {
            _bpm = Mathf.Clamp(newBPM, 30, 300);
            UpdateBeatInterval();

            nextBeatTime = Time.time + beatInterval;
        }

        private void PlayTick()
        {
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }

        private void UpdateBeatInterval()
        {
            beatInterval = 60f / _bpm;
        }
    }
}