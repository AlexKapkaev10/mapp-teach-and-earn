using Project.Code.Scripts.Module.Mining;
using VContainer;

namespace Project.Code.Scripts.Architecture
{
    public class GameScope : BaseScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            RegisterMining(builder);
        }

        private void RegisterMining(IContainerBuilder builder)
        {
            builder.Register<MiningModel>(Lifetime.Singleton)
                .As<IMiningModel>();
            
            builder.Register<MiningPresenter>(Lifetime.Singleton)
                .As<IMiningPresenter>();
        }
    }
}