using System.Collections;
using System.Collections.Generic;
using Project.Configs;
using Project.Scripts.Audio;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class DrumMachineView : View
    {
        [SerializeField] private DrumMachineConfig _config;
        
        [SerializeField] private FuncItem _recItem;
        [SerializeField] private FuncItem _playItem;

        [SerializeField] private TimerItem _timerItem;
        [SerializeField] private GameObject _padsParent;

        private readonly List<ButtonPressData> _pressDataList = new ();
        private IDrumMachineItem[] _pads;
        private Coroutine _playCoroutine;
        private Coroutine _timerCoroutine;
        private Metronome _metronome;
        
        private float _startRecTime;
        private float _endRecTime;
        
        private float _elapsedTime;
        
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
            if (!_isPC)
            {
                return;
            }
            
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

        protected override void OnEnable()
        {
            _recItem.Button.onClick.AddListener(StartRecording);
            _playItem.Button.onClick.AddListener(PlayRecordedSequence);
        }

        private void OnDisable()
        {
            _recItem.Button.onClick.RemoveListener(StartRecording);
            _playItem.Button.onClick.RemoveListener(PlayRecordedSequence);
        }

        private void SetMetronome()
        {
            _isMetronome = !_isMetronome;

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

            _pressDataList.Clear();
            _startRecTime = Time.time;
            
            _timerItem.SetHeader("rec");
            _timerCoroutine = StartCoroutine(StartTimerAsync());
            _recItem.SetColorIndicator(_config.ColorIndicatorActive);
        }

        private void StopRecording()
        {
            foreach (var pad in _pads)
            {
                pad.Clicked -= RegisterButtonPress;
            }

            _recItem.SetColorIndicator(_config.ColorIndicatorDefault);
            
            _isRecording = false;
            
            StopCoroutine(_timerCoroutine);
            
            _timerItem.SetRecCount(_pressDataList.Count > 0 ? "1" : "0");
            _timerItem.SetHeader();
            _timerItem.UpdateTimer();
            
            _endRecTime = Time.time - _startRecTime;
        }

        private void PlayRecordedSequence()
        {
            if (_isRecording)
            {
                StopRecording();
            }
            
            if (_pressDataList.Count == 0)
            {
                return;
            }

            if (_playCoroutine != null)
            {
                StopPlaying();
                return;
            }
            
            
            _timerItem.SetHeader("play");
            _timerCoroutine = StartCoroutine(StartTimerAsync());
            _playItem.SetColorIndicator(_config.ColorIndicatorActive);
            
            _playCoroutine = StartCoroutine(PlaySequence());
        }

        private void RegisterButtonPress(int buttonIndex)
        {
            if (!_isRecording)
            {
                return;
            }

            float currentTime = Time.time - _startRecTime;
            _pressDataList.Add(new ButtonPressData(buttonIndex, currentTime));
        }

        private void StopPlaying()
        {
            _playItem.SetColorIndicator(_config.ColorIndicatorDefault);
            StopCoroutine(_playCoroutine);
            StopCoroutine(_timerCoroutine);
            _timerItem.SetHeader();
            _timerItem.UpdateTimer();
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

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_elapsedTime / 60);
            int seconds = Mathf.FloorToInt(_elapsedTime % 60);
            
            _timerItem.UpdateTimer($"{minutes:00}:{seconds:00}");
        }

        private IEnumerator StartTimerAsync()
        {
            _elapsedTime = 0;
            
            while (true)
            {
                _elapsedTime += Time.deltaTime;
                UpdateTimerText();
                yield return null;
            }
        }

        private IEnumerator PlaySequence()
        {
            float playbackStartTime = Time.time;

            var waitAfter = _endRecTime - _pressDataList[^1].Time;

            foreach (var press in _pressDataList)
            {
                float delay = press.Time - (Time.time - playbackStartTime);

                if (delay > 0)
                {
                    yield return new WaitForSeconds(delay);
                }

                PlayButtonSound(press.ButtonIndex);
            }

            yield return new WaitForSeconds(waitAfter);
            
            StopPlaying();
        }
    }
}