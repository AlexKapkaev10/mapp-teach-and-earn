namespace Project.Scripts.Audio
{
    [System.Serializable]
    public class ButtonPressData
    {
        public int ButtonIndex;
        public float Time;

        public ButtonPressData(int buttonIndex, float time)
        {
            ButtonIndex = buttonIndex;
            Time = time;
        }
    }
}