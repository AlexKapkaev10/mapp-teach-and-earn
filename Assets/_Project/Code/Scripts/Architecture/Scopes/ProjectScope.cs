using Project.Scripts.Bank;
using Project.Scripts.Scene;
using Project.Scripts.Skills;
using Project.Code.Scripts.API;
using Project.Scripts.Architecture;
using Project.Scripts.Factory;
using Project.Scripts.Tools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Code.Scripts.Architecture
{
    public class ProjectScope : BaseScope
    {
        [SerializeField] private SkillsServiceConfig _skillsServiceConfig;
        [SerializeField] private CoroutineStarter _coroutineStarter;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInNewPrefab(_coroutineStarter, Lifetime.Singleton)
                .As<ICoroutineStarter>();

            builder.RegisterEntryPoint<ClientAPI>()
                .As<IClientAPI>();
            
            RegisterBank(builder);
            RegisterServices(builder);
        }
        
        private void RegisterBank(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<Bank>()
                .As<IBank>();
        }
        
        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<SceneService>(Lifetime.Singleton)
                .As<ISceneService>();
            
            builder.Register<SaveLoadService>(Lifetime.Singleton)
                .As<ISaveLoadService>();
            
            builder.Register<SkillsService>(Lifetime.Singleton)
                .As<ISkillsService>()
                .WithParameter(_skillsServiceConfig);
            
            builder.Register<Factory>(Lifetime.Singleton)
                .As<IFactory>();
        }
    }
}