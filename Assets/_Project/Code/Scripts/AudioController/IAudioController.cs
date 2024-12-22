namespace Project.Scripts.Audio
{
    public interface IAudioController
    {
        void SetFxClip(in AudioMode mode);
        void SetAmbientClip(in AudioMode clip);
        void PlayFXClip();
        void PlayAmbientClip();
    }
}