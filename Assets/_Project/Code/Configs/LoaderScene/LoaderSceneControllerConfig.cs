using _Project.Scripts.Loaders;
using UnityEngine;

namespace _Project.Configs.LoaderScene
{
    [CreateAssetMenu(fileName = nameof(LoaderSceneControllerConfig), menuName = "Configs/Scene/LoaderSceneControllerConfig")]
    public class LoaderSceneControllerConfig : ScriptableObject
    {
        [field: SerializeField] public LoaderView LoaderView { get; private set; }
        [field: SerializeField] public string NextScene { get; private set; } = "";
    }
}