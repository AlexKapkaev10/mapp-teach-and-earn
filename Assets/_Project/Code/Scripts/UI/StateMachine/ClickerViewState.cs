using System.Collections.Generic;
using Project.Scripts.Services;

namespace Project.Scripts.UI.StateMachine
{
    public class ClickerViewState : IViewState
    {
        private readonly List<View> _views = new List<View>();
        private readonly ViewsStateMachineConfig _config;
        private readonly IFactory _factory;
        
        public ClickerViewState(IFactory factory, ViewsStateMachineConfig config)
        {
            _config = config;
            _factory = factory;
        }
        
        public void Enter()
        {
            var clicker = _factory.GetView(_config.ClickerViewPrefab);
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