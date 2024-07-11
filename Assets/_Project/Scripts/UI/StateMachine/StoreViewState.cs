using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.UI.StateMachine
{
    public class StoreViewState : IViewState
    {
        private readonly List<View> _views = new List<View>();
        private readonly ViewsStateMachineConfig _config;
        private readonly IObjectResolver _resolver;
        
        public StoreViewState(IObjectResolver resolver, ViewsStateMachineConfig config)
        {
            _resolver = resolver;
            _config = config;
        }
        
        public void Enter()
        {
            var store = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.Store), null);
            var gameInfo = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.GameInfo), null);
            
            _views.Add(store);
            _views.Add(gameInfo);
        }

        public void Exit()
        {
            foreach (var view in _views)
            {
                view.SetDisable();
            }
            
            _views.Clear();
        }
    }
}