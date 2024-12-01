using Cysharp.Threading.Tasks;
using Project.Scripts.Architecture;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Localization
{
    public interface ILocalizationService : IInitializable
    {
        void SwitchLocale(int localeId);
    }
    
    public class LocalizationService : ILocalizationService
    {
        private readonly ISaveLoadService _saveLoadService;

        [Inject]
        public LocalizationService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }
        
        public async void Initialize()
        {
            await LocalizationSettings.InitializationOperation;
            SwitchLocale(_saveLoadService.GetLocaleID());
        }

        public void SwitchLocale(int localeId)
        {
            var targetLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
            LocalizationSettings.SelectedLocale = targetLocale;
        }
    }
}