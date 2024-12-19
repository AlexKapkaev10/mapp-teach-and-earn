using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.UI.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.UI
{
    public interface IViewsStateMachine : IInitializable
    {
        void SwitchViewByType(ViewStateType type);
    }
    
    public class ViewsStateMachine : IViewsStateMachine
    {
        private Dictionary<ViewStateType, IViewState> _dictionaryStates;
        private ViewsStateMachineConfig _config;

        private IFactory _factory;
        private IViewState _currentViewState;
        private IAssetResourceService _resourceService;

        [Inject]
        private void Construct(IFactory factory, IAssetResourceService resourceService, ViewsStateMachineConfig config)
        {
            _factory = factory;
            _resourceService = resourceService;
            _config = config;
        }

        public void Initialize()
        {
            if (_config.IsCheckFps)
            {
                _factory.GetView(_config.FpsViewPrefab.GetComponent<View>());
            }
            
            _resourceService.LoadGameObjectByReference(_config.SwitchViewReference, OnSwitchMenuLoaded);
        }

        private void OnSwitchMenuLoaded(GameObject prefab)
        {
            _factory.GetView(prefab.GetComponent<View>());
            
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
                { ViewStateType.Home , new HomeViewState(_factory, _config)},
                { ViewStateType.Activity , new ActivityViewState(_factory, _config)},
                { ViewStateType.Clicker , new ClickerViewState(_factory, _config)},
                { ViewStateType.Quest , new QuestViewState(_factory, _config)},
                { ViewStateType.DramMachine , new DramMachineViewState(_factory, _config)}
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
        Quest,
        DramMachine
    }
}