using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Audio
{
    public class FuncItem : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Image ImageIndicator { get; private set; }

        public void SetColorIndicator(in Color color)
        {
            ImageIndicator.color = color;
        }
    }
}