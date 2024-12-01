using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.UI
{
    [CreateAssetMenu(fileName = nameof(ViewsStateMachineConfig), menuName = "Configs/UI/StateMachine/ViewsStateMachineConfig")]
    public class ViewsStateMachineConfig : ScriptableObject
    {
        [SerializeField] private View[] _viewPrefabs;
        [field: SerializeField] public bool IsCheckFps { get; private set; } = true;
        [field: SerializeField] public AssetReference Test { get; private set; }
        [field: SerializeField] public AssetReference MainViewReference { get; private set; }
        [field: SerializeField] public AssetReference InfoViewReference { get; private set; }

        public View[] ViewPrefabs => _viewPrefabs;

        public View GetViewPrefabByType(ViewType type)
        {
            foreach (var prefab in _viewPrefabs)
            {
                if (prefab.ViewType == type)
                {
                    return prefab;
                }
            }

            return null;
        }
    }
}