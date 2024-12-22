using System.Collections;
using System.Collections.Generic;
using Project.Infrastructure.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Audio
{
    public class DrumMachineController : MonoBehaviour
    {
        [SerializeField] private GameObject _padsParent;
        [SerializeField] private Image _recIndicator;
        [SerializeField] private Image _playIndicator;
        [SerializeField] private Image _metronomeIndicator;
        [SerializeField] private Color _colorIndicatorDefault;
        [SerializeField] private Color _colorIndicatorActive = Color.red;

        [SerializeField] private Button _buttonMetronome;
        [SerializeField] private GameObject _metronomeObject;

        private readonly List<ButtonPressData> _pressDataList = new ();
        private Coroutine _playCoroutine;
        private IDrumMachineItem[] _pads;
        private float _startTime;
        private bool _isRecording;
        private bool _isMetronome;

        private void Awake()
        {
            _pads = _padsParent.GetComponentsInChildren<IDrumMachineItem>();
        }

        private void OnEnable()
        {
            _buttonMetronome.onClick.AddListener(SetMetronome);
        }

        private void OnDisable()
        {
            _buttonMetronome.onClick.RemoveListener(SetMetronome);
        }

        private void SetMetronome()
        {
            _isMetronome = !_isMetronome;

            _metronomeIndicator.color = _isMetronome ? _colorIndicatorActive : _colorIndicatorDefault;
            
            _metronomeObject.SetActive(_isMetronome);
        }

        public void StartRecording()
        {
            if (_isRecording)
            {
                this.Log("Recording process...");
                return;
            }

            _isRecording = true;
            foreach (var pad in _pads)
            {
                pad.Clicked += RegisterButtonPress;
            }
            
            if (_playCoroutine != null)
            {
                StopPlaying();
            }

            _recIndicator.color = _colorIndicatorActive;
            _pressDataList.Clear();
            _startTime = Time.time;
            this.Log("Recording started...");
        }

        public void StopRecording()
        {
            if (!_isRecording)
            {
                this.LogWarning("Recording is not active. Nothing to stop.");
                return;
            }
            
            foreach (var pad in _pads)
            {
                pad.Clicked -= RegisterButtonPress;
            }

            _recIndicator.color = _colorIndicatorDefault;
            float elapsedTime = Time.time - _startTime;
            _isRecording = false;
            this.Log($"Recording stopped. Total time: {elapsedTime}s");
        }

        public void PlayRecordedSequence()
        {
            if (_isRecording)
            {
                StopRecording();
            }
            
            if (_pressDataList.Count == 0)
            {
                this.LogWarning("Can not play.");
                return;
            }

            if (_playCoroutine != null)
            {
                StopPlaying();
            }

            _playIndicator.color = _colorIndicatorActive;

            this.Log("Playing recorded sequence...");
            _playCoroutine = StartCoroutine(PlaySequence());
        }

        private void RegisterButtonPress(int buttonIndex)
        {
            if (!_isRecording)
            {
                this.LogWarning("Recording is not active. Button press ignored.");
                return;
            }

            float currentTime = Time.time - _startTime;
            _pressDataList.Add(new ButtonPressData(buttonIndex, currentTime));
            this.Log($"Button {buttonIndex} pressed at {currentTime}s");
        }

        private void StopPlaying()
        {
            _playIndicator.color = _colorIndicatorDefault;
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
        }

        private void PlayButtonSound(int buttonIndex)
        {
            foreach (var item in _pads)
            {
                if (item.Index == buttonIndex)
                {
                    item.PlayClip();
                    break;
                }
            }
        }

        private IEnumerator PlaySequence()
        {
            float playbackStartTime = Time.time;

            foreach (var press in _pressDataList)
            {
                float delay = press.Time - (Time.time - playbackStartTime);
                
                if (delay > 0)
                {
                    yield return new WaitForSeconds(delay);
                }
                
                PlayButtonSound(press.ButtonIndex);
            }

            StopPlaying();
        }
    }
}