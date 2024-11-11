using Project.Configs.LoaderScene;
using Project.Scripts.Scene;
using UnityEngine;
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

        private void Start()
        {
            _loaderView = Instantiate(_config.LoaderView, null);
            _loaderView.LoadComplete += OnLoadComplete;
            _loaderView.StartSlider();
        }

        private void OnLoadComplete()
        {
            _loaderView.LoadComplete -= OnLoadComplete;
            _sceneService.LoadSceneByName(_config.NextScene);
        }
    }
}