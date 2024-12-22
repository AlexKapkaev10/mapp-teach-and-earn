using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public class WebGLAudioRecorder : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void startRecording();

    [DllImport("__Internal")]
    private static extern void stopRecording();

    private void Awake()
    {
        transform.SetParent(null);
    }

    public void StartRecording()
    {
        startRecording();
    }

    public void StopRecording()
    {
        stopRecording();
    }
    
    public void OnRecordingComplete(string jsonData)
    {
        // Распакуем строку в массив байтов
        byte[] audioData = JsonUtility.FromJson<ByteArrayWrapper>(jsonData).Data;

        Debug.Log($"Received audio data of length: {audioData.Length}");
        // Здесь можно обработать данные, например, сохранить или воспроизвести
    }

    [Serializable]
    private class ByteArrayWrapper
    {
        public byte[] Data;
    }
}