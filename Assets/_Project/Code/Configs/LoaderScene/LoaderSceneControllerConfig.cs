using Project.Scripts.Loader;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Configs.LoaderScene
{
    [CreateAssetMenu(fileName = nameof(LoaderSceneControllerConfig), menuName = "Configs/Scene/LoaderSceneControllerConfig")]
    public class LoaderSceneControllerConfig : ScriptableObject
    {
        [field: SerializeField] public string NextScene { get; private set; } = "";
        [field: SerializeField] public AssetLabelReference PreloadLabel { get; private set; }
    }
}