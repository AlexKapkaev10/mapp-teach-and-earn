using System.Collections.Generic;
using Project.Scripts.Services;
using VContainer;

namespace Project.Scripts.UI.StateMachine
{
    public class QuestViewState : IViewState
    {
        private readonly List<View> _views = new ();
        private readonly ViewsStateMachineConfig _config;
        private readonly IFactory _factory;

        public QuestViewState(IFactory factory, ViewsStateMachineConfig config)
        {
            _factory = factory;
            _config = config;
        }
        
        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }
}