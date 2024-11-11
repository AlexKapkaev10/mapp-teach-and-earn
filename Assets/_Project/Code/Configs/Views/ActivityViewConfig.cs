using System;
using UnityEngine;

namespace Project.Scripts.UI.Activity
{
    
    [CreateAssetMenu(fileName = nameof(ActivityViewConfig), menuName = "Configs/Views/ActivityViewConfig")]
    public class ActivityViewConfig : ScriptableObject
    {
        [field: SerializeField] public ActivityButtonData[] _activityButtonsData;
        
        [field: SerializeField] public ActivityButton _buttonPrefab;
        [field: SerializeField] public Sprite _spriteLock;
    }
    
    [Serializable]
    public struct ActivityButtonData
    {
        public Sprite SpriteIcon;
        public ViewStateType SwitchToStateType;
        public string HeaderLocalizationKey;
        public string DescriptionLocalizationKey;
    }
}