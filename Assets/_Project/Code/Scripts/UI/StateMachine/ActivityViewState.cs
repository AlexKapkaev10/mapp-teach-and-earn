using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.UI.StateMachine
{
    public class ActivityViewState : IViewState
    {
        private readonly List<View> _views = new List<View>();
        private readonly ViewsStateMachineConfig _config;
        private readonly IObjectResolver _resolver;
        
        public ActivityViewState(IObjectResolver resolver, ViewsStateMachineConfig config)
        {
            _config = config;
            _resolver = resolver;
        }
        
        public void Enter()
        {
            var clicker = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.Activity), null);
            _views.Add(clicker);
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