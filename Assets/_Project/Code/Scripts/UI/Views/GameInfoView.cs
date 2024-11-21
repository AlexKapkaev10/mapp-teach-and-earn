using Project.Scripts.Bank;
using TMPro;
using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public class GameInfoView : View
    {
        [SerializeField] private TMP_Text _textCoins;
        
        private IBank _presenter;

        [Inject]
        private void Construct(IBank presenter)
        {
            _presenter = presenter;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateCoinsText(_presenter.GetCoins());
            _presenter.CoinValueChanged += UpdateCoinsText;
        }

        private void OnDisable()
        {
            _presenter.CoinValueChanged -= UpdateCoinsText;
        }

        private void UpdateCoinsText(int coinsText)
        {
            _textCoins.SetText(coinsText.ToString());
        }
    }
}

