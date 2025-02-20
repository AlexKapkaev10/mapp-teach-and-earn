using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Project.Scripts.UI.Views
{
    public interface IQuestView { }

    public class QuestView : View, IQuestView
    {
        private const string ServerUrl = "https://ght-test.ngrok.app"; 
        private string _videoPath;

        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _rawImage;
        
        private RenderTexture _renderTexture;

        private async void Start()
        {
            _renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            _videoPlayer.targetTexture = _renderTexture;
            _rawImage.texture = _renderTexture;
            
            string fileName = "video.mp4";
            string serverVideoUrl = $"https://ght-test.ngrok.app/uploads/{fileName}";
            string localVideoPath = Path.Combine(Application.persistentDataPath, fileName);
            
            await PlayVideo(localVideoPath, serverVideoUrl);
        }
        
        public async UniTask PlayVideo(string localPath, string serverUrl)
        {
            if (_videoPlayer == null)
            {
                Debug.LogError("❌ Ошибка: VideoPlayer не существует!");
                return;
            }
            
            Debug.Log($"🎬 Воспроизводим видео из Интернета: {serverUrl}");
            _videoPlayer.url = serverUrl; // Используем URL на сервере

            _videoPlayer.Prepare();
            await UniTask.WaitUntil(() => _videoPlayer.isPrepared);
            _videoPlayer.Play();
        }
    }
}
