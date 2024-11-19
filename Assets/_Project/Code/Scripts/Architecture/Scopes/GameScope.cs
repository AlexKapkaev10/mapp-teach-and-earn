using Project.Code.Configs.Views;
using Project.Scripts.Audio;
using Project.Scripts.UI;
using Project.Code.Scripts.Module.Mining;
using Project.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Code.Scripts.Architecture
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
            RegisterMining(builder);
            RegisterFactory(builder);
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

        private void RegisterFactory(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<ViewsStateMachine>()
                .As<IViewsStateMachine>()
                .WithParameter(_viewsStateMachineConfig);
        }
    }
}