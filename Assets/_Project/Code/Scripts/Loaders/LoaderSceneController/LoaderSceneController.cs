using _Project.Configs.LoaderScene;
using _Project.Scripts.Bank;
using _Project.Scripts.Scene;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Loaders
{
    public class LoaderSceneController : MonoBehaviour
    {
        [SerializeField] private LoaderSceneControllerConfig _config;
        private ILoaderView _loaderView;
        private ISceneService _sceneService;
        private IBankPresenter _bankPresenter;
        
        [Inject]
        private void Construct(ISceneService sceneService, IBankPresenter bankPresenter)
        {
            _sceneService = sceneService;
            _bankPresenter = bankPresenter;
        }

        private void Start()
        {
            _bankPresenter.InitModel();
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