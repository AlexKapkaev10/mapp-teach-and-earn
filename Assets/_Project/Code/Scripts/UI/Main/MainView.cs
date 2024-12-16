using Project.Scripts.UI;
using Project.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Scripts.Mining
{
    public interface IMainView
    {
        void UpdateScore(string scoreText);
        void UpdateClaimButton(bool isActive);
        void UpdateLog(string log, bool isSuccess);
        void ClearLogs();
    }
    
    public sealed class MainView : View, IMainView
    {
        [SerializeField] private Button _buttonClaim;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _buttonUpgradeRandom;
        [SerializeField] private Button _buttonConnectWallet;
        [SerializeField] private Button _buttonDisconnectWallet;

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
            _buttonUpgradeRandom.onClick.AddListener(OnUpgradeRandomClick);
            _buttonConnectWallet.onClick.AddListener(OnConnectWalletClick);
            _buttonDisconnectWallet.onClick.AddListener(OnDisconnectWalletClick);

            UpdateClaimButton(_presenter.CanClaim);
        }

        private void OnDisconnectWalletClick()
        {
            _presenter.DisconnectWallet();
        }

        private void OnConnectWalletClick()
        {
            _presenter.ConnectWallet();
        }

        private void OnDisable()
        {
            _buttonClaim.onClick.RemoveListener(OnClaimClick);
            _buttonBuy.onClick.RemoveListener(OnBuyClick);
            _buttonUpgradeRandom.onClick.RemoveListener(OnUpgradeRandomClick);
            
            _presenter.Dispose();
        }

        private void OnClaimClick()
        {
            _presenter.ClaimClick();
        }

        private void OnBuyClick()
        {
            _presenter.Buy();
        }

        private void OnUpgradeRandomClick()
        {
            _presenter.UpgradeRandom();
        }

        public void UpdateScore(string scoreText)
        {
            _textScore.SetText(scoreText);
        }

        public void UpdateClaimButton(bool isActive)
        {
            _buttonClaim.gameObject.SetActive(isActive);
        }

        public void UpdateLog(string log, bool isSuccess)
        {
            _textLog.SetText(log);
            _textLog.color = isSuccess ? Color.green : Color.red;
        }

        public void ClearLogs()
        {
            _textLog.SetText("");
        }
    }
}