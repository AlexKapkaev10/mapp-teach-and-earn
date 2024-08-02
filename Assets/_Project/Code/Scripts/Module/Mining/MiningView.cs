using Project.Code.Scripts.View;
using Project.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Code.Scripts.Module.Mining
{
    public interface IMiningView
    {
        void UpdateScore(string scoreText);
    }
    
    public sealed class MiningView : BaseView, IMiningView
    {
        [SerializeField] private TMP_Text _textScore;
        [SerializeField] private Button _buttonClaim;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _buttonUpgrade;
        [SerializeField] private TransactionHandler _transactionHandler;
        
        private IMiningPresenter _presenter;
        
        [Inject]
        private void Construct(IMiningPresenter presenter)
        {
            _presenter = presenter;
            _presenter.Init(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _buttonClaim.onClick.AddListener(OnClaimClick);
            _buttonBuy.onClick.AddListener(OnBuyClick);
            _buttonUpgrade.onClick.AddListener(OnUpgradeClick);
            
            _transactionHandler.TransactionSend += OnTransactionSend;
        }

        private void OnTransactionSend()
        {
            _presenter.TransactionSend();
        }

        private void OnDisable()
        {
            _buttonClaim.onClick.RemoveListener(OnClaimClick);
            _buttonBuy.onClick.RemoveListener(OnBuyClick);
            _buttonUpgrade.onClick.RemoveListener(OnUpgradeClick);
            
            _transactionHandler.TransactionSend -= OnTransactionSend;
        }

        private void OnClaimClick()
        {
            _presenter.Claim();
        }

        private void OnBuyClick()
        {
            _presenter.Buy();
        }

        private void OnUpgradeClick()
        {
            _presenter.Upgrade();
        }

        public void UpdateScore(string scoreText)
        {
            _textScore.SetText(scoreText);
        }
    }
}