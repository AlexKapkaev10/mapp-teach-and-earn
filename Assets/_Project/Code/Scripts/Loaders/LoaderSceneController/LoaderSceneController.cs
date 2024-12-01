using System.Threading.Tasks;
using Project.Configs.LoaderScene;
using Project.Scripts.Scene;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace Project.Scripts.Loader
{
    public class LoaderSceneController : MonoBehaviour
    {
        [SerializeField] private LoaderSceneControllerConfig _config;
        
        private ILoaderView _loaderView;
        private ISceneService _sceneService;

        [Inject]
        private void Construct(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        private async void Start()
        {
            await LoadAllAssets();
            
            _loaderView = Instantiate(_config.LoaderView, null);
            _loaderView.LoadComplete += OnLoadComplete;
            _loaderView.StartSlider();
        }

        private void OnLoadComplete()
        {
            _loaderView.LoadComplete -= OnLoadComplete;
            _sceneService.LoadSceneByName(_config.NextScene);
        }

        private async Task LoadAllAssets()
        {
            foreach (var assetReference in _config.References)
            {
                try
                {
                    var handle = assetReference.LoadAssetAsync<Object>();
                    await handle.Task;

                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log($"Succeeded: {assetReference.RuntimeKey}");
                    }
                    else
                    {
                        Debug.LogError($"Fail: {assetReference.RuntimeKey}");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Fail load {assetReference.RuntimeKey}: {ex.Message}");
                }
            }
        }
    }
}