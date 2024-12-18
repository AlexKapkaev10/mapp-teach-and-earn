using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.UI
{
    [CreateAssetMenu(fileName = nameof(ViewsStateMachineConfig), menuName = "Configs/UI/StateMachine/ViewsStateMachineConfig")]
    public class ViewsStateMachineConfig : ScriptableObject
    {
        [SerializeField] private View[] _viewPrefabs;
        [field: SerializeField] public bool IsCheckFps { get; private set; } = true;
        [field: SerializeField] public AssetReference MainViewReference { get; private set; }
        [field: SerializeField] public AssetReference ActivityViewReference { get; private set; }
        [field: SerializeField] public AssetReference InfoViewReference { get; private set; }
        [field: SerializeField] public AssetReference SwitchViewReference { get; private set; }
        [field: SerializeField] public AssetReference DramMachineViewReference { get; private set; }
        //ToDo: Replace to AssetReference
        [field: SerializeField] public View ClickerViewPrefab { get; private set; }
        [field: SerializeField] public View FpsViewPrefab { get; private set; }
    }
}