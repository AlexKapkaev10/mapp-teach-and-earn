using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
/*using YoutubeExplode;
using YoutubeExplode.Videos.Streams;*/

namespace _Project.Scripts.Tools
{
    public class VideoPlayerTool : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private string videoUrl = "https://drive.google.com/uc?export=download&id=1PfR8t9NQ7sWN8RFRjT5-l-hQBcd0pWhm";

        private async void Start()
        {
            //await PlayYouTubeVideo(videoUrl);
        }

        /*private async Task PlayYouTubeVideo(string url)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            Debug.Log(video.Id);
            try
            {
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);
                Debug.Log(streamManifest);

                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
                
                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                var filePath = $"{Application.temporaryCachePath}/{video}.mp4";
                await youtube.Videos.Streams.DownloadAsync(streamInfo, filePath);
                Debug.Log(streamManifest);
                videoPlayer.url = filePath;
                videoPlayer.Play();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }


        }*/
    }
}