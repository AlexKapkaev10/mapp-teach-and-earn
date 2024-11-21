using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Tools;
using Project.Scripts.UI.StateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.UI
{
    public interface IViewsStateMachine
    {
        void SwitchStateByType(ViewStateType type);
    }
    
    public class ViewsStateMachine : MonoBehaviour, IViewsStateMachine
    {
        private ViewsStateMachineConfig _config = default;
        private Dictionary<ViewStateType, IViewState> _dictionaryStates;
        
        private IViewState _currentViewState = default;
        
        private ISwitchViewMenu _switchViewMenu;
        private IObjectResolver _resolver = default;
        private ICoroutineStarter _coroutineStarter;

        [Inject]
        private void Construct(
            IObjectResolver resolver, 
            ICoroutineStarter coroutineStarter,
            ViewsStateMachineConfig config)
        {
            _resolver = resolver;
            _coroutineStarter = coroutineStarter;
            _config = config;
        }

        private void Awake()
        {
            if (_config.IsCheckFps)
            {
                Instantiate(_config.GetViewPrefabByType(ViewType.CheckFPS), null);
            }

            _switchViewMenu = _resolver
                .Instantiate(_config.GetViewPrefabByType(ViewType.SwitchViewMenu), null) 
                as SwitchViewMenu;
            
            CreateMachine();

            StartCoroutine(LoadPrefabAsync(_config.Test));
        }

        public void SwitchStateByType(ViewStateType type)
        {
            if (!_dictionaryStates.TryGetValue(type, out IViewState viewState))
            {
                return;
            }
            
            if (_currentViewState == viewState)
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

        private IEnumerator LoadPrefabAsync(AssetReference assetReference)
        {
            var handle = assetReference.LoadAssetAsync<GameObject>();
            
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _resolver.Instantiate(handle.Result, null);
            }
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