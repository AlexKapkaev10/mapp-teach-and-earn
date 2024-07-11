using _Project.Scripts.Bank;
using TMPro;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI
{
    public class GameInfoView : View
    {
        [SerializeField] private TMP_Text _textCoins;
        
        private IBankPresenter _presenter;

        [Inject]
        private void Construct(IBankPresenter presenter)
        {
            _presenter = presenter;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateCoinsText(_presenter.GetGameCoinCount());
            _presenter.GameCoinValueChange += UpdateCoinsText;
        }

        private void OnDisable()
        {
            _presenter.GameCoinValueChange -= UpdateCoinsText;
        }

        public void UpdateCoinsText(string coinsText)
        {
            _textCoins.SetText(coinsText);
        }
    }
}

