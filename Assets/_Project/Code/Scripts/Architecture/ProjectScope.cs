using Project.Code.Scripts.API;
using VContainer;

namespace Project.Code.Scripts.Architecture
{
    public class ProjectScope : BaseScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<TestAPI>(Lifetime.Singleton)
                .As<ITestAPI>();
        }
    }
}