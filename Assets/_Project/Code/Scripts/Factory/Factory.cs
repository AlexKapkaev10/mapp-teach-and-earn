using Project.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Services
{
    public interface IFactory
    {
        IObjectResolver Resolver { get; }

        T GetView<T>(in T component, in Transform parent = null) 
            where T : View;
    }
    
    public sealed class Factory : IFactory
    {
        private readonly IObjectResolver _resolver;
        public IObjectResolver Resolver => _resolver;

        [Inject]
        public Factory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T GetView<T>(in T component, in Transform parent = null) where T : View
        {
            return _resolver.Instantiate(component, parent);
        }
    }
}