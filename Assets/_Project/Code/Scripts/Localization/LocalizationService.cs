using System.Collections;
using Project.Scripts.Architecture;
using Project.Scripts.Tools;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Localization
{
    public interface ILocalizationService : IInitializable
    {
        
    }
    
    public class LocalizationService : ILocalizationService
    {
        private readonly ICoroutineStarter _coroutineStarter;
        private readonly ISaveLoadService _saveLoadService;

        [Inject]
        public LocalizationService(
            ICoroutineStarter coroutineStarter,
            ISaveLoadService saveLoadService)
        {
            _coroutineStarter = coroutineStarter;
            _saveLoadService = saveLoadService;
        }
        
        public void Initialize()
        {
            _coroutineStarter.Starter.StartCoroutine(SwitchLocaleAsync(_saveLoadService.GetLocaleID()));
        }

        private IEnumerator SwitchLocaleAsync(int localeId)
        {
            yield return LocalizationSettings.InitializationOperation;

            var targetLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
            LocalizationSettings.SelectedLocale = targetLocale;
            
            yield return LocalizationSettings.InitializationOperation;
        }
    }
}