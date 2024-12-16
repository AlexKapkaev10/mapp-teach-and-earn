using Project.Scripts.Scene;
using Project.Scripts.Services;
using VContainer;

namespace Project.Scripts.Loader
{
    public interface ILoaderService
    {
        void StartLoadResources();
    }
    
    public class LoaderService : ILoaderService
    {
        private readonly IAssetResourceService _resourceService;
        private readonly LoaderServiceConfig _config;
        private readonly ISceneService _sceneService;

        [Inject]
        public LoaderService(
            IAssetResourceService resourceService, 
            ISceneService sceneService, 
            LoaderServiceConfig config)
        {
            _config = config;
            _resourceService = resourceService;
            _sceneService = sceneService;
        }

        public void StartLoadResources()
        {
            _resourceService.LoadAssetsByLabel(_config.PreloadLabel, OnLoadResources);
        }

        private void OnLoadResources()
        {
            _sceneService.LoadSceneByName(_config.NextScene);
        }
    }
}