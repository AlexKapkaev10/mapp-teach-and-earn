using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.UI.StateMachine
{
    public class HomeViewState : IViewState
    {
        private readonly List<View> _views = new List<View>();
        private readonly ViewsStateMachineConfig _config;
        private readonly IObjectResolver _resolver;
        
        public HomeViewState(IObjectResolver resolver, ViewsStateMachineConfig config)
        {
            _config = config;
            _resolver = resolver;
        }
        
        public void Enter()
        {
            var character = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.Character), null);
            var gameInfo = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.GameInfo), null);
            _views.Add(character);
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