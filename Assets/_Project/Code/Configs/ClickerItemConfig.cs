using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = nameof(ClickerItemConfig), menuName = "Configs/Clicker/ClickerItemConfig")]
    public class ClickerItemConfig : ScriptableObject
    {
        [field: SerializeField] public FlyItem FlyItemPrefab { get; private set; }
        [field: SerializeField] public int PoolCount { get; private set; } = 5;
        [field: SerializeField] public bool AutoExpandPool { get; private set; } = true;
        [field: SerializeField] public float EndScaleValue { get; private set; } = 1.1f;
        
        [field: SerializeField] public float EndScaleDuration { get; private set; } = 0.05f;
        
        [SerializeField] private float _flyItemOffsetY = 20f;

        public Vector2 GetFlyItemOffset()
        {
            return Vector2.up * _flyItemOffsetY;
        }
    }
}