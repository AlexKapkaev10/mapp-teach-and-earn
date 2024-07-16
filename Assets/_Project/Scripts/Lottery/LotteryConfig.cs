using Project.Lottery;
using UnityEngine;

namespace Project.Configs
{
    [CreateAssetMenu(fileName = nameof(LotteryConfig), menuName = "Configs/LotteryConfig")]
    public class LotteryConfig : ScriptableObject
    {
        [field: SerializeField] public LotteryItem LotteryItemPrefab { get; private set; }
    }
}