using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Infrastructure.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Project.Scripts.Services
{
    public interface IAssetResourceService
    {
        void LoadAssetsByLabel(AssetLabelReference label, Action callBack);
        void LoadGameObjectByReference(AssetReference label, Action<GameObject> callBack);
    }
    
    public class AssetResourceService : IAssetResourceService
    {
        public async void LoadAssetsByLabel(AssetLabelReference label, Action callBack)
        {
            await LoadAssetsAsync(label);
            callBack?.Invoke();
        }

        public async void LoadGameObjectByReference(AssetReference assetReference, Action<GameObject> callBack)
        {
            await LoadGameObjectAsync(assetReference, callBack);
        }

        private async Task LoadAssetsAsync(AssetLabelReference label)
        {
            try
            {
                var handle = Addressables.LoadAssetsAsync<Object>(label.labelString);
                
                await handle.Task;
                
                this.Log(handle.Result as MonoBehaviour);

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    this.Log($"All assets with label '{label}' loaded successfully.");
                }
                else
                {
                    this.LogError($"Failed to load assets with label '{label}'.");
                }
            }
            catch (Exception ex)
            {
                this.LogError($"Error loading assets with label '{label}': {ex.Message}");
            }
        }
        
        private async Task LoadGameObjectAsync(AssetReference assetReference, Action<GameObject> calBack)
        {
            try
            {
                var reference = await assetReference.LoadAssetAsync<GameObject>();
                calBack?.Invoke(reference);
            }
            catch (Exception ex)
            {
                this.LogError($"Error loading asset reference '{assetReference}': {ex.Message}");
            }
        }
    }
}