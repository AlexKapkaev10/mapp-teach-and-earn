using UnityEngine;

namespace Project.Code.Configs.Views
{
    [CreateAssetMenu(fileName = nameof(MainPresenterConfig), menuName = "Configs/UI/StateMachine/MainPresenterConfig")]
    public class MainPresenterConfig : ScriptableObject
    {
        [field: SerializeField] public float ClearLogsDelay { get; private set; } = 1f;
    }
}