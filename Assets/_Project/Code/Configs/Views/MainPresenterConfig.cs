using UnityEngine;

namespace Project.Configs.Views
{
    [CreateAssetMenu(fileName = nameof(MainPresenterConfig), menuName = "Configs/UI/StateMachine/MainPresenterConfig")]
    public class MainPresenterConfig : ScriptableObject
    {
        [field: SerializeField] public string Success { get; private set; } = "Success";
        [field: SerializeField] public string Error { get; private set; } = "Error";
        [field: SerializeField] public string POI { get; private set; } = "POI";
    }
}