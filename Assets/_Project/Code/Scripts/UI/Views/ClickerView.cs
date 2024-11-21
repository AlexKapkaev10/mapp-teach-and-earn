using Project.Scripts.Audio;
using Project.Scripts.Bank;
using TMPro;
using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public interface IClickerView
    {
        
    }
    
    public class ClickerView : View, IClickerView
    {
        [SerializeField] private Coin _coin;
        [SerializeField] private TMP_Text _textCoinCount;
        
        private IBank _bank = default;
        private IAudioController _audioController = default;
        
        [Inject]
        private void Construct(IAudioController audioController, IBank bank)
        {
            _audioController = audioController;
            _bank = bank;
        }

        private void Awake()
        {
            _audioController.SetFxClip(AudioMode.ClickCoin);
            _audioController.SetAmbientClip(AudioMode.ClickerAmbient);
            _audioController.PlayAmbientClip();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateCoinsText(_bank.GetCoins());
            
            _bank.CoinValueChanged += UpdateCoinsText;
            _coin.ClickItem += CoinOnCoinClick;
        }

        private void OnDisable()
        {
            _bank.CoinValueChanged -= UpdateCoinsText;
            _coin.ClickItem -= CoinOnCoinClick;
        }

        private void UpdateCoinsText(int coinsValue)
        {
            _textCoinCount.SetText(coinsValue.ToString());
        }

        private void CoinOnCoinClick()
        {
            _bank.SetCoins(1);
            _audioController.PlayFXClip();
            _coin.ClickAnimation();
        }
    }
}