using System.Collections;
using System.Collections.Generic;
using Project.Configs;
using Project.Infrastructure.Extensions;
using Project.Scripts.Audio;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class DrumMachineView : View
    {
        [SerializeField] private DrumMachineConfig _config;
        
        [SerializeField] private FuncItem _recItem;
        [SerializeField] private FuncItem _playItem;
        [SerializeField] private FuncItem _metronomeItem;

        [SerializeField] private GameObject _padsParent;

        private readonly List<ButtonPressData> _pressDataList = new ();
        private IDrumMachineItem[] _pads;
        private Coroutine _playCoroutine;
        private Metronome _metronome;
        private float _startTime;
        private bool _isRecording;
        private bool _isMetronome;
        private bool _isPC;

        private void Awake()
        {
            _pads = _padsParent.GetComponentsInChildren<IDrumMachineItem>();
            
            for (var i = 0; i < _pads.Length; i++)
            {
                var pad = _pads[i];
                pad.SetIndex(i);
            }
            
            _isPC = !Application.isMobilePlatform;
        }

        private void Update()
        {
            if (_isPC)
            {
                for (int i = 0; i < _pads.Length; i++)
                {
                    if (Input.GetKeyDown(_config.GetKeyCode(i)))
                    {
                        _pads[i].OnPointerDown(null);
                    }
                    if (Input.GetKeyUp(_config.GetKeyCode(i)))
                    {
                        _pads[i].OnPointerUp(null);
                    }
                }
            }
        }

        protected override void OnEnable()
        {
            _recItem.Button.onClick.AddListener(StartRecording);
            _playItem.Button.onClick.AddListener(PlayRecordedSequence);
            _metronomeItem.Button.onClick.AddListener(SetMetronome);
        }

        private void OnDisable()
        {
            _recItem.Button.onClick.RemoveListener(StartRecording);
            _playItem.Button.onClick.RemoveListener(PlayRecordedSequence);
            _metronomeItem.Button.onClick.RemoveListener(SetMetronome);
        }

        private void SetMetronome()
        {
            _isMetronome = !_isMetronome;

            _metronomeItem.SetColorIndicator(_isMetronome
                ? _config.ColorIndicatorActive
                : _config.ColorIndicatorDefault);

            if (_isMetronome)
            {
                _metronome = Instantiate(_config.MetronomePrefab, null);
            }
            else
            {
                Destroy(_metronome.gameObject);
            }
        }

        public void StartRecording()
        {
            if (_isRecording)
            {
                StopRecording();
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

            _recItem.SetColorIndicator(_config.ColorIndicatorActive);
            _pressDataList.Clear();
            _startTime = Time.time;
            this.Log("Recording started...");
        }

        private void StopRecording()
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

            _recItem.SetColorIndicator(_config.ColorIndicatorDefault);
            float elapsedTime = Time.time - _startTime;
            _isRecording = false;
            this.Log($"Recording stopped. Total time: {elapsedTime}s");
        }

        private void PlayRecordedSequence()
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
                return;
            }

            _playItem.SetColorIndicator(_config.ColorIndicatorActive);

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
            _playItem.SetColorIndicator(_config.ColorIndicatorDefault);
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