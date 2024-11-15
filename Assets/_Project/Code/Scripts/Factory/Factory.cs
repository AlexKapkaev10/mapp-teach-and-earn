using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Factory
{
    public interface IFactory
    {
        public T CreateFromResolver<T>(in T component, in Transform parent = null) 
            where T : MonoBehaviour;
    }
    
    public sealed class Factory : IFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public Factory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T CreateFromResolver<T>(in T component, in Transform parent = null) where T : MonoBehaviour
        {
            return _resolver.Instantiate(component, parent);
        }
    }
}