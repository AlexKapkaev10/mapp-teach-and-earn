using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.UI.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.UI
{
    public interface IViewsStateMachine
    {
        void SwitchViewByType(ViewStateType type);
    }
    
    public class ViewsStateMachine : MonoBehaviour, IViewsStateMachine
    {
        private Dictionary<ViewStateType, IViewState> _dictionaryStates;
        private ViewsStateMachineConfig _config;

        private IObjectResolver _resolver;
        private IViewState _currentViewState;
        private IAssetResourceService _resourceService;

        [Inject]
        private void Construct(
            IObjectResolver resolver,
            IAssetResourceService resourceService,
            ViewsStateMachineConfig config)
        {
            _resolver = resolver;
            _resourceService = resourceService;
            _config = config;
        }

        private void Awake()
        {
            if (_config.IsCheckFps)
            {
                Instantiate(_config.GetViewPrefabByType(ViewType.CheckFPS), null);
            }
            
            _resourceService.LoadGameObjectByReference(_config.SwitchViewReference, OnSwitchMenuLoaded);
        }

        private void OnSwitchMenuLoaded(GameObject prefab)
        {
            _resolver.Instantiate(prefab, null);
            
            CreateMachine();
        }

        public void SwitchViewByType(ViewStateType type)
        {
            if (!_dictionaryStates.TryGetValue(type, out var viewState) || _currentViewState == viewState)
            {
                return;
            }

            if (_currentViewState != null)
            {
                _currentViewState.Exit();
                _currentViewState = null;
            }
            
            _currentViewState = viewState;
            _currentViewState.Enter();
        }

        private void CreateMachine()
        {
            _dictionaryStates = new Dictionary<ViewStateType, IViewState>
            {
                { ViewStateType.Home , new HomeViewState(_resolver, _config)},
                { ViewStateType.Activity , new ActivityViewState(_resolver, _config)},
                { ViewStateType.Clicker , new ClickerViewState(_resolver, _config)},
                { ViewStateType.Quest , new QuestViewState(_resolver, _config)}
            };
        }
    }

    public enum ViewStateType
    {
        None,
        Home,
        Activity,
        Clicker,
        Farm,
        Quest
    }
}