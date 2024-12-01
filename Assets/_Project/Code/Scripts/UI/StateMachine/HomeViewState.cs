using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.UI.StateMachine
{
    public class HomeViewState : IViewState
    {
        private readonly List<View> _views = new ();

        private CancellationTokenSource _cts;
        private readonly ViewsStateMachineConfig _config;
        private readonly IObjectResolver _resolver;

        public HomeViewState(IObjectResolver resolver, ViewsStateMachineConfig config)
        {
            _config = config;
            _resolver = resolver;
        }
        
        public async void Enter()
        {
            _cts = new CancellationTokenSource();
            
            await LoadViewAsync(_config.MainViewReference, _cts.Token);
            await LoadViewAsync(_config.InfoViewReference, _cts.Token);
            
            /*var main = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.Main), null);
            var gameInfo = _resolver.Instantiate(_config.GetViewPrefabByType(ViewType.GameInfo), null);
            
            _views.Add(main);
            _views.Add(gameInfo);*/
        }

        public void Exit()
        {
            _cts.Cancel();
            _cts = null;
            
            foreach (var view in _views)
            {
                view.SetDisable();
            }
            
            _views.Clear();
        }
        
        private async Task LoadViewAsync(AssetReference assetReference, CancellationToken token)
        {
            try
            {
                var reference = await assetReference.LoadAssetAsync<GameObject>();

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            
                var view = _resolver.Instantiate(reference, null)
                    .GetComponent<View>();
                
                _views.Add(view);
                assetReference.ReleaseAsset();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        }
    }
}