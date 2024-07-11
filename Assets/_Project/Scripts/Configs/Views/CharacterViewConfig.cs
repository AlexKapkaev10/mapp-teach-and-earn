using _Project.Scripts.GameCharacter;
using UnityEngine;

namespace _Project.Scripts.Configs.Views
{
    [CreateAssetMenu(fileName = nameof(CharacterViewConfig), menuName = "Configs/Views/CharacterViewConfig")]
    public class CharacterViewConfig : ScriptableObject
    {
        [field: SerializeField] public Character CharacterPrefab { get; private set; }
    }
}