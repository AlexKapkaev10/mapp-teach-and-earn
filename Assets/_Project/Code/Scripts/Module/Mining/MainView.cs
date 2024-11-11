using Project.Scripts.UI;
using Project.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Code.Scripts.Module.Mining
{
    public interface IMainView
    {
        void UpdateScore(string scoreText);
        void UpdateLog(string log);
    }
    
    public sealed class MainView : View, IMainView
    {
        [SerializeField] private Button _buttonClaim;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _buttonUpgrade;
        [SerializeField] private TMP_Text _textScore;
        [SerializeField] private TMP_Text _textLog;

        private ITransactionHandler _transactionHandler;
        
        private IMainPresenter _presenter;
        
        [Inject]
        private void Construct(IMainPresenter presenter)
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
        }

        private void OnDisable()
        {
            _buttonClaim.onClick.RemoveListener(OnClaimClick);
            _buttonBuy.onClick.RemoveListener(OnBuyClick);
            _buttonUpgrade.onClick.RemoveListener(OnUpgradeClick);
            _presenter.Dispose();
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

        public void UpdateLog(string log)
        {
            _textLog.SetText(log);
        }
    }
}