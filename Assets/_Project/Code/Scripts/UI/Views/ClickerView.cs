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
        
        private IBankPresenter _bankPresenter = default;
        private IAudioController _audioController = default;
        
        [Inject]
        private void Construct(IAudioController audioController, IBankPresenter bankPresenter)
        {
            _audioController = audioController;
            _bankPresenter = bankPresenter;
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
            UpdateCoinsText(_bankPresenter.GetGameCoinCount());
            _bankPresenter.GameCoinValueChange += UpdateCoinsText;
            _coin.ClickItem += CoinOnCoinClick;
        }

        private void OnDisable()
        {
            _bankPresenter.GameCoinValueChange -= UpdateCoinsText;
            _coin.ClickItem -= CoinOnCoinClick;
        }

        private void UpdateCoinsText(string textCoinCount)
        {
            _textCoinCount.SetText(textCoinCount);
        }

        private void CoinOnCoinClick()
        {
            _bankPresenter.AddGameCoin(1);
            _audioController.PlayFXClip();
            _coin.ClickAnimation();
        }
    }
}