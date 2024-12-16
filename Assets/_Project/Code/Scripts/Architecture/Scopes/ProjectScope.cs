using Project.Scripts.Bank;
using Project.Scripts.Scene;
using Project.Scripts.Skills;
using Project.Scripts.API;
using Project.Scripts.Connect;
using Project.Scripts.Factory;
using Project.Scripts.Loader;
using Project.Scripts.Localization;
using Project.Scripts.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Architecture
{
    public class ProjectScope : BaseScope
    {
        [SerializeField] private SkillsServiceConfig _skillsServiceConfig;
        [SerializeField] private TelegramConnectServiceConfig _telegramConnectServiceConfig;
        [SerializeField] private LoaderServiceConfig _loaderServiceConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterEntryPoint<ClientAPI>()
                .As<IClientAPI>();
            
            RegisterBank(builder);
            RegisterServices(builder);
        }
        
        private void RegisterBank(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<Bank.Bank>()
                .As<IBank>();
        }
        
        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<TelegramConnectService>(Lifetime.Singleton)
                .As<ITelegramConnectService>()
                .WithParameter(_telegramConnectServiceConfig);
            
            builder.Register<LoaderService>(Lifetime.Singleton)
                .As<ILoaderService>()
                .WithParameter(_loaderServiceConfig);
            
            builder.Register<AssetResourceService>(Lifetime.Singleton)
                .As<IAssetResourceService>();
            
            builder.RegisterEntryPoint<LocalizationService>()
                .As<ILocalizationService>();
            
            builder.Register<SceneService>(Lifetime.Singleton)
                .As<ISceneService>();
            
            builder.Register<SaveLoadService>(Lifetime.Singleton)
                .As<ISaveLoadService>();
            
            builder.Register<SkillsService>(Lifetime.Singleton)
                .As<ISkillsService>()
                .WithParameter(_skillsServiceConfig);
            
            builder.Register<Factory.Factory>(Lifetime.Singleton)
                .As<IFactory>();
        }
    }
}