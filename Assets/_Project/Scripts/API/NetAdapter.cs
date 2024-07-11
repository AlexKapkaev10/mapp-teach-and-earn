using UnityEngine;
using VContainer;

namespace _Project.Scripts.API
{
    public class NetAdapter : MonoBehaviour
    {
        private IApiClient _apiClient;
        [SerializeField] private string _getUrl = "https://jsonplaceholder.typicode.com/posts/1";
        [SerializeField] private string _postUrl = "https://jsonplaceholder.typicode.com/posts";
        
        [Inject]
        private void Configure(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        private async void Start()
        {
            PostDto getPostDtoResponse = await _apiClient.GetPostAsync(_getUrl);
            if (getPostDtoResponse != null)
            {
                Debug.Log($"Title: {getPostDtoResponse.title}, Body: {getPostDtoResponse.body}");
            }

            string json = "{\"title\":\"foo\",\"body\":\"bar\",\"userId\":1}";
            PostDto postDtoPostDtoResponse = await _apiClient.PostPostAsync(_postUrl, json);
            if (postDtoPostDtoResponse != null)
            {
                Debug.Log($"ID: {postDtoPostDtoResponse.id}, Title: {postDtoPostDtoResponse.title}, Body: {postDtoPostDtoResponse.body}");
            }
        }
    }
}