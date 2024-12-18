using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Project.Infrastructure.Extensions;
using Project.Scripts.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.UI.StateMachine
{
    public class ActivityViewState : IViewState
    {
        private readonly ViewsStateMachineConfig _config;
        private readonly List<View> _views = new ();
        private readonly IFactory _factory;
        
        private CancellationTokenSource _cts;

        public ActivityViewState(IFactory factory, ViewsStateMachineConfig config)
        {
            _config = config;
            _factory = factory;
        }
        
        public async void Enter()
        {
            _cts = new CancellationTokenSource();

            await LoadViewAsync(_config.ActivityViewReference, _cts.Token);
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
                var reference = await assetReference.LoadAssetAsync<GameObject>().Task;
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                var view = _factory.GetView(reference.GetComponent<View>());
                _views.Add(view);
                assetReference.ReleaseAsset();
            }
            catch (Exception e)
            {
                this.Log(e.Message);
            }
        }
    }
}