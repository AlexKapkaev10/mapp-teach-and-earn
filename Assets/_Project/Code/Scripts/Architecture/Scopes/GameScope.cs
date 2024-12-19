using Project.Scripts.Audio;
using Project.Scripts.UI;
using Project.Configs.Views;
using Project.Scripts.Mining;
using Project.Scripts.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Architecture
{
    public class GameScope : BaseScope
    {
        [SerializeField] private ViewsStateMachineConfig _viewsStateMachineConfig;
        [SerializeField] private AudioControllerConfig _audioControllerConfig;
        [SerializeField] private MainPresenterConfig _mainPresenterConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponentInHierarchy<ITransactionHandler>();

            builder.Register<Factory>(Lifetime.Singleton)
                .As<IFactory>();
            
            RegisterMining(builder);
            RegisterViewStateMachine(builder);
            RegisterAudioSystem(builder);
        }

        private void RegisterMining(IContainerBuilder builder)
        {
            builder.Register<MainModel>(Lifetime.Singleton)
                .As<IMainModel>();
            
            builder.Register<MainPresenter>(Lifetime.Singleton)
                .As<IMainPresenter>()
                .WithParameter(_mainPresenterConfig);
        }
        
        private void RegisterAudioSystem(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<AudioController>()
                .As<IAudioController>()
                .WithParameter(_audioControllerConfig);
        }

        private void RegisterViewStateMachine(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ViewsStateMachine>()
                .As<IViewsStateMachine>()
                .WithParameter(_viewsStateMachineConfig);
        }
    }
}