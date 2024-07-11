using _Project.Scripts.API;
using _Project.Scripts.Audio;
using _Project.Scripts.Bank;
using _Project.Scripts.SaveLoad;
using _Project.Scripts.Scene;
using _Project.Scripts.Skills;
using _Project.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Core
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private ViewsStateMachineConfig _viewsStateMachineConfig;
        [SerializeField] private AudioControllerConfig _audioControllerConfig;
        [SerializeField] private SkillsServiceConfig _skillsServiceConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            RegisterApi(builder);
            RegisterBank(builder);
            RegisterServices(builder);
            RegisterFactory(builder);
            RegisterAudioSystem(builder);
        }

        private void RegisterApi(IContainerBuilder builder)
        {
            builder.Register<ApiClient>(Lifetime.Singleton)
                .As<IApiClient>();
        }

        private void RegisterAudioSystem(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<AudioController>()
                .As<IAudioController>()
                .WithParameter(_audioControllerConfig);
        }

        private void RegisterFactory(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<ViewsStateMachine>()
                .As<IViewsStateMachine>()
                .WithParameter(_viewsStateMachineConfig);
        }
        
        private void RegisterBank(IContainerBuilder builder)
        {
            builder.Register<BankModel>(Lifetime.Singleton)
                .As<IBankModel>();
            builder.Register<BankPresenter>(Lifetime.Singleton)
                .As<IBankPresenter>();
        }
        
        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<SceneService>(Lifetime.Singleton)
                .As<ISceneService>();
            
            builder.Register<SaveLoadServiceSimple>(Lifetime.Singleton)
                .As<ISaveLoadService>();
            
            builder.Register<SkillsService>(Lifetime.Singleton)
                .As<ISkillsService>()
                .WithParameter(_skillsServiceConfig);
        }
    }
}