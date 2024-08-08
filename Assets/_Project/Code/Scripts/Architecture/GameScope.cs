using _Project.Scripts.Audio;
using _Project.Scripts.UI;
using Project.Code.Scripts.Module.Mining;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Code.Scripts.Architecture
{
    public class GameScope : BaseScope
    {
        [SerializeField] private ViewsStateMachineConfig _viewsStateMachineConfig;
        [SerializeField] private AudioControllerConfig _audioControllerConfig; 
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            RegisterMining(builder);
            RegisterFactory(builder);
            RegisterAudioSystem(builder);
        }

        private void RegisterMining(IContainerBuilder builder)
        {
            builder.Register<MiningModel>(Lifetime.Singleton)
                .As<IMiningModel>();
            
            builder.Register<MiningPresenter>(Lifetime.Singleton)
                .As<IMiningPresenter>();
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