using Project.Scripts.Loader;
using UnityEngine;

namespace Project.Configs.LoaderScene
{
    [CreateAssetMenu(fileName = nameof(LoaderSceneControllerConfig), menuName = "Configs/Scene/LoaderSceneControllerConfig")]
    public class LoaderSceneControllerConfig : ScriptableObject
    {
        [field: SerializeField] public LoaderView LoaderView { get; private set; }
        [field: SerializeField] public string NextScene { get; private set; } = "";
    }
}