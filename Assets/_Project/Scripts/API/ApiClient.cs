using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace _Project.Scripts.API
{
    public interface IApiClient
    {
        Task<PostDto> GetPostAsync(string url);
        Task<PostDto> PostPostAsync(string url, string json);
    }
    
    public class ApiClient : IApiClient
    {
        public async Task<PostDto> GetPostAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Request error: {request.error}");
                    return null;
                }

                string responseBody = request.downloadHandler.text;
                PostDto postDto = JsonConvert.DeserializeObject<PostDto>(responseBody);
                return postDto;
            }
        }

        public async Task<PostDto> PostPostAsync(string url, string json)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Request error: {request.error}");
                    return null;
                }

                string responseBody = request.downloadHandler.text;
                PostDto postDto = JsonConvert.DeserializeObject<PostDto>(responseBody);
                return postDto;
            }
        }
    }
    
    [Serializable]
    public class PostDto
    {
        public int userId;
        public int id;
        public string title;
        public string body;
    }
}