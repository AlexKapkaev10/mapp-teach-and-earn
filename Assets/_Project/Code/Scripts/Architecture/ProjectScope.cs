using Project.Scripts.Bank;
using Project.Scripts.SaveLoad;
using Project.Scripts.Scene;
using Project.Scripts.Skills;
using Project.Code.Scripts.API;
using Project.Scripts.Factory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Code.Scripts.Architecture
{
    public class ProjectScope : BaseScope
    {
        [SerializeField] private SkillsServiceConfig _skillsServiceConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<TestAPI>(Lifetime.Singleton)
                .As<ITestAPI>();
            
            RegisterBank(builder);
            RegisterServices(builder);
        }
        
        private void RegisterBank(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BankModel>()
                .As<IBankModel>();
            
            builder.RegisterEntryPoint<BankPresenter>()
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
            
            builder.Register<Factory>(Lifetime.Singleton)
                .As<IFactory>();
        }
    }
}