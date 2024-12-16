using UnityEngine;

namespace Project.Scripts.Connect
{
    [CreateAssetMenu(fileName = nameof(TelegramConnectServiceConfig), menuName = "Configs/Services/Telegram Connect")]
    public class TelegramConnectServiceConfig : ScriptableObject
    {
        [field: SerializeField, TextArea] public string EditorInitData { get; private set; }
        [field: SerializeField] public string ManifestUrl { get; private set; }
        [field: SerializeField] public string ServerUrl { get; private set; } = "https://inv-my.ngrok.app/validate";
        [field: SerializeField] public string DataKey { get; private set; } = "initData";
        [field: SerializeField] public string ContentTypeKey { get; private set; } = "Content-Type";
        [field: SerializeField] public string ApplicationKey { get; private set; } = "application/json";
    }
}