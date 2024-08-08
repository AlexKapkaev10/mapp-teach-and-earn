using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.UI.StateMachine
{
    public class QuestViewState : IViewState
    {
        private readonly List<View> _views = new List<View>();
        private readonly ViewsStateMachineConfig _config;
        private readonly IObjectResolver _resolver;

        public QuestViewState(IObjectResolver resolver, ViewsStateMachineConfig config)
        {
            _resolver = resolver;
            _config = config;
        }
        
        public void Enter()
        {
            Debug.Log("Quest Enter");
            var quest = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.Quest), null);
            _views.Add(quest);
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