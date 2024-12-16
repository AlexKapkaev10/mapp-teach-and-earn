using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Loader
{
    [CreateAssetMenu(fileName = nameof(LoaderServiceConfig), menuName = "Configs/Service/Loader")]
    public class LoaderServiceConfig : ScriptableObject
    {
        [field: SerializeField] public string NextScene { get; private set; } = "";
        [field: SerializeField] public AssetLabelReference PreloadLabel { get; private set; }
    }
}