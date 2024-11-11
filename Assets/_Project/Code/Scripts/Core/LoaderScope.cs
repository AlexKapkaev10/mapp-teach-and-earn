using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Core
{
    public class LoaderScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }
    }
}