using _Project.Scripts.Bank;
using _Project.Scripts.SaveLoad;
using _Project.Scripts.Scene;
using _Project.Scripts.Skills;
using Project.Code.Scripts.API;
using UnityEngine;
using VContainer;

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