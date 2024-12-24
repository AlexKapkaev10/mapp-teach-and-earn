using Project.Scripts;
using UnityEngine;

namespace Project.Configs
{
    [CreateAssetMenu(fileName = nameof(DrumMachineConfig), menuName = "Configs/Services/Drum Machine")]
    public class DrumMachineConfig : ScriptableObject
    {
        [field: SerializeField] public Color ColorIndicatorDefault { get; private set; }
        [field: SerializeField] public Color ColorIndicatorActive { get; private set; } = Color.red;
        [field: SerializeField] public Metronome MetronomePrefab { get; private set; }
        
        [SerializeField] private string[] _keyCodes;

        public string GetKeyCode(int index)
        {
            if (index >= 0 && index < _keyCodes.Length)
            {
                return _keyCodes[index];
            }

            return null;
        }
    }
}