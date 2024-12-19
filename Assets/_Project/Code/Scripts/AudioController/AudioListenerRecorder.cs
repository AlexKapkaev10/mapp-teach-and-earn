using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Audio
{
    public class AudioListenerRecorder : MonoBehaviour
    {
        private readonly List<byte[]> _audioDataList = new List<byte[]>(); // Лист для хранения байтов аудио
        private MemoryStream _memoryStream;
        private bool _isRecording = false;
        private int _outputRate;
        private int _recordedChannels;
        private const int k_headerSize = 44;

        [SerializeField] private AudioSource _audioSource;

        private void Start()
        {
            _outputRate = AudioSettings.outputSampleRate;
        }

        public void StartRecording()
        {
            if (_isRecording)
            {
                Debug.LogWarning("Recording is already in progress.");
                return;
            }

            _memoryStream = new MemoryStream();
            WriteEmptyHeader(); // Записываем пустой заголовок WAV
            _isRecording = true;
            Debug.Log("Recording started.");
        }

        private void StopRecording()
        {
            if (!_isRecording)
            {
                Debug.LogWarning("No recording in progress to stop.");
                return;
            }

            if (_recordedChannels <= 0)
            {
                Debug.LogError("Invalid number of channels in WAV header.");
                return;
            }

            _isRecording = false;
            WriteHeader(_recordedChannels);

            byte[] recordedData = _memoryStream.ToArray();
            _audioDataList.Add(recordedData);

            _memoryStream.Dispose();
            _memoryStream = null;

            Debug.Log($"Recording stopped. Audio saved to memory. Total recordings: {_audioDataList.Count}");
            
            PlayRecordingAsync(_audioDataList.Count - 1);
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (!_isRecording) return;

            if (channels <= 0)
            {
                Debug.LogError("Invalid number of channels during recording.");
                return;
            }

            _recordedChannels = channels;
            Debug.Log($"Channels detected: {_recordedChannels}");

            NormalizeData(data);
            ConvertAndWrite(data);
        }

        private void NormalizeData(float[] data)
        {
            float maxAmplitude = 0f;

            foreach (var sample in data)
            {
                maxAmplitude = Mathf.Max(maxAmplitude, Mathf.Abs(sample));
            }

            if (maxAmplitude > 1f)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] /= maxAmplitude;
                }
            }
        }

        private void ConvertAndWrite(float[] dataSource)
        {
            Int16[] intData = new Int16[dataSource.Length];
            byte[] bytesData = new byte[dataSource.Length * 2];
            float rescaleFactor = 32767;

            for (int i = 0; i < dataSource.Length; i++)
            {
                intData[i] = (Int16)(Mathf.Clamp(dataSource[i], -1f, 1f) * rescaleFactor);
                bytesData[i * 2] = (byte)(intData[i] & 0xFF);
                bytesData[i * 2 + 1] = (byte)((intData[i] >> 8) & 0xFF);
            }

            _memoryStream.Write(bytesData, 0, bytesData.Length);
        }

        private void WriteEmptyHeader()
        {
            byte[] emptyByte = new byte[k_headerSize];
            _memoryStream.Write(emptyByte, 0, k_headerSize);
        }

        private void WriteHeader(int channels)
        {
            if (channels <= 0)
            {
                channels = 2;
            }

            _memoryStream.Seek(0, SeekOrigin.Begin);

            byte[] riff = ToByteArray("RIFF");
            _memoryStream.Write(riff, 0, 4);

            byte[] chunkSize = BitConverter.GetBytes((int)(_memoryStream.Length - 8));
            _memoryStream.Write(chunkSize, 0, 4);

            byte[] wave = ToByteArray("WAVE");
            _memoryStream.Write(wave, 0, 4);

            byte[] fmt = ToByteArray("fmt ");
            _memoryStream.Write(fmt, 0, 4);

            byte[] subChunk1 = BitConverter.GetBytes(16);
            _memoryStream.Write(subChunk1, 0, 4);

            UInt16 audioFormat = 1; // PCM
            _memoryStream.Write(BitConverter.GetBytes(audioFormat), 0, 2);

            UInt16 numChannels = (UInt16)channels;
            _memoryStream.Write(BitConverter.GetBytes(numChannels), 0, 2);

            byte[] sampleRate = BitConverter.GetBytes(_outputRate);
            _memoryStream.Write(sampleRate, 0, 4);

            byte[] byteRate = BitConverter.GetBytes(_outputRate * channels * 2);
            _memoryStream.Write(byteRate, 0, 4);

            UInt16 blockAlign = (UInt16)(channels * 2);
            _memoryStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bitsPerSample = 16;
            _memoryStream.Write(BitConverter.GetBytes(bitsPerSample), 0, 2);

            byte[] dataString = ToByteArray("data");
            _memoryStream.Write(dataString, 0, 4);

            byte[] subChunk2Size = BitConverter.GetBytes((int)(_memoryStream.Length - k_headerSize));
            _memoryStream.Write(subChunk2Size, 0, 4);
        }

        private byte[] ToByteArray(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = (byte)str[i];
            }
            return bytes;
        }

        private async void PlayRecordingAsync(int index)
        {
            if (index < 0 || index >= _audioDataList.Count)
            {
                Debug.LogError("Invalid recording index.");
                return;
            }

            byte[] audioBytes = _audioDataList[index];
            AudioClip clip = ConvertToAudioClip(audioBytes);

            if (clip == null)
            {
                Debug.LogError("Failed to convert byte array to AudioClip.");
                return;
            }

            await PlayWhenReadyAsync(clip);
        }

        private async Task PlayWhenReadyAsync(AudioClip clip)
        {
            while (clip.loadState != AudioDataLoadState.Loaded)
            {
                await Task.Yield(); // Асинхронно ждем загрузки клипа
            }

            _audioSource.clip = clip;
            _audioSource.Play();
            Debug.Log("Audio is playing...");
        }

        private AudioClip ConvertToAudioClip(byte[] wavData)
        {
            const int headerSize = 44;
            if (wavData.Length < headerSize)
            {
                Debug.LogError("Invalid WAV data: too short.");
                return null;
            }

            int sampleRate = BitConverter.ToInt32(wavData, 24);
            int channels = BitConverter.ToInt16(wavData, 22);

            if (channels <= 0 || sampleRate <= 0)
            {
                Debug.LogError($"Invalid WAV header. SampleRate: {sampleRate}, Channels: {channels}");
                return null;
            }

            int samples = (wavData.Length - headerSize) / 2;

            float[] audioData = new float[samples];
            for (int i = 0; i < samples; i++)
            {
                short sample = BitConverter.ToInt16(wavData, headerSize + i * 2);
                audioData[i] = sample / 32768f;
            }

            AudioClip audioClip = AudioClip.Create("GeneratedAudio", samples, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);

            return audioClip;
        }
    }
}
