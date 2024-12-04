using System.Collections;
using Project.Configs.LoaderScene;
using Project.Scripts.Scene;
using Project.Scripts.Services;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Scripts.Loader
{
    public class LoaderView : View
    {
        [SerializeField] private LoaderSceneControllerConfig _config;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _loadSimulationSpeed = 0.002f;
        
        private ISceneService _sceneService;
        private IAssetResourceService _assetResourceService;

        [Inject]
        private void Construct(ISceneService sceneService, IAssetResourceService assetResourceService)
        {
            _sceneService = sceneService;
            _assetResourceService = assetResourceService;
        }
        
        private void Awake()
        {
            _assetResourceService.LoadAssetsByLabel(_config.PreloadLabel, StartSlider);
        }
        
        public void StartSlider()
        {
            StartCoroutine(LoadSimulationAsync());
        }

        private IEnumerator LoadSimulationAsync()
        {
            while (_slider.value < 1)
            {
                _slider.value += _loadSimulationSpeed * Time.unscaledTime;
                yield return null;
            }
            
            _sceneService.LoadSceneByName(_config.NextScene);
        }
    }
}