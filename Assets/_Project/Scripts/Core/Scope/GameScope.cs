using Project.Configs;
using Project.Lottery;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Scope
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private LotteryConfig _lotteryConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<LotteryModel>(Lifetime.Singleton)
                .As<ILotteryModel>();
            
            builder.Register<LotteryPresenter>(Lifetime.Singleton)
                .As<ILotteryPresenter>()
                .WithParameter(_lotteryConfig);
        }
    }
}