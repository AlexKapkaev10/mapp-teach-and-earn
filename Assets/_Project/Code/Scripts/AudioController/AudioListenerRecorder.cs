using System;
using System.IO;
using UnityEngine;

namespace Project.Scripts.Audio
{
    public class AudioListenerRecorder : MonoBehaviour
    {
        private FileStream _fileStream;
        private bool _isRecording = false;
        private int _outputRate;
        private string _fileName;
        private int _recordedChannels;
        private const int k_headerSize = 44;

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
            
            _fileName = Path.Combine(Application.persistentDataPath, "audio_listener_record.wav");
            StartWriting(_fileName);
            _isRecording = true;
            Debug.Log($"Recording started. File path: {_fileName}");
        }

        public void StopRecording()
        {
            if (!_isRecording)
            {
                Debug.LogWarning("No recording in progress to stop.");
                return;
            }

            _isRecording = false;
            WriteHeader(_recordedChannels);
            Debug.Log($"Recording stopped. File saved: {_fileName}");
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (!_isRecording) return;

            _recordedChannels = channels;
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

            _fileStream.Write(bytesData, 0, bytesData.Length);
        }

        private void StartWriting(string fileName)
        {
            _fileStream = new FileStream(fileName, FileMode.Create);
            byte[] emptyByte = new byte[k_headerSize];
            _fileStream.Write(emptyByte, 0, k_headerSize);
        }

        private void WriteHeader(int channels)
        {
            _fileStream.Seek(0, SeekOrigin.Begin);

            byte[] riff = ToByteArray("RIFF");
            _fileStream.Write(riff, 0, 4);

            byte[] chunkSize = BitConverter.GetBytes((int)(_fileStream.Length - 8));
            _fileStream.Write(chunkSize, 0, 4);

            byte[] wave = ToByteArray("WAVE");
            _fileStream.Write(wave, 0, 4);

            byte[] fmt = ToByteArray("fmt ");
            _fileStream.Write(fmt, 0, 4);

            byte[] subChunk1 = BitConverter.GetBytes(16);
            _fileStream.Write(subChunk1, 0, 4);

            UInt16 audioFormat = 1; // PCM
            _fileStream.Write(BitConverter.GetBytes(audioFormat), 0, 2);

            UInt16 numChannels = (UInt16)channels;
            _fileStream.Write(BitConverter.GetBytes(numChannels), 0, 2);

            byte[] sampleRate = BitConverter.GetBytes(_outputRate);
            _fileStream.Write(sampleRate, 0, 4);

            byte[] byteRate = BitConverter.GetBytes(_outputRate * channels * 2);
            _fileStream.Write(byteRate, 0, 4);

            UInt16 blockAlign = (UInt16)(channels * 2);
            _fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bitsPerSample = 16;
            _fileStream.Write(BitConverter.GetBytes(bitsPerSample), 0, 2);

            byte[] dataString = ToByteArray("data");
            _fileStream.Write(dataString, 0, 4);

            byte[] subChunk2Size = BitConverter.GetBytes((int)(_fileStream.Length - k_headerSize));
            _fileStream.Write(subChunk2Size, 0, 4);

            _fileStream.Close();
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
    }

}