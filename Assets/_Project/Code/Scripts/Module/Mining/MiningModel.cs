using System.Runtime.InteropServices;
using Project.Code.Scripts.API;

namespace Project.Code.Scripts.Module.Mining
{
    public interface IMiningModel
    {
        float Score { get; }
        void SetInit();
        void Claim(out float claimValue);
        void Upgrade();
        void Buy();
        void TransactionSend();
    }
    
    public class MiningModel : IMiningModel
    {
        [DllImport("__Internal")]
        private static extern void BuyForStars();
    
        [DllImport("__Internal")]
        private static extern void Init();
    
        [DllImport("__Internal")]
        private static extern void Send();
        
        private readonly ITestAPI _testAPI;
        private float _score;

        public float Score => _testAPI.GetScore();

        public MiningModel(ITestAPI testAPI)
        {
            _testAPI = testAPI;
        }

        public void SetInit()
        {
#if !UNITY_EDITOR
            Init();
#endif
        }

        public void Claim(out float claimValue)
        {
            claimValue = _testAPI.GetRandomClaim();
        }

        public void Upgrade()
        {
#if !UNITY_EDITOR
            BuyForStars();
#endif
        }
    
        public void Buy()
        {
#if !UNITY_EDITOR
            Send();
#endif
        }

        public void TransactionSend()
        {
            _testAPI.TransactionSend();
        }
    }
}