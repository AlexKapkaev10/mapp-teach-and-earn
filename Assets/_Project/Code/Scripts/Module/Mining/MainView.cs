using System.Collections;
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
        void UpdateClaimButton(bool isActive);
        void UpdateLog(string log, bool isSuccess);
        void ClearLogs();
    }
    
    public sealed class MainView : View, IMainView
    {
        [SerializeField] private Button _buttonClaim;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _buttonUpgrade;
        
        [SerializeField] private TMP_Text _textScore;
        [SerializeField] private TMP_Text _textLog;
        [SerializeField] private TMP_Text _textButtonClaim;
        [SerializeField] private float _delayValue = 3f;

        private ITransactionHandler _transactionHandler;

        private IMainPresenter _presenter;

        private Coroutine _clearLogsRoutine;
        private WaitForSeconds _clearLogsDelay;
        
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

            _clearLogsDelay = new WaitForSeconds(_delayValue);
            
            UpdateClaimButton(_presenter.CanClaim);
        }

        private void OnDisable()
        {
            _buttonClaim.onClick.RemoveListener(OnClaimClick);
            _buttonBuy.onClick.RemoveListener(OnBuyClick);
            _buttonUpgrade.onClick.RemoveListener(OnUpgradeClick);

            _clearLogsDelay = null;
            
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

        public void UpdateClaimButton(bool isActive)
        {
            if (!isActive)
            {
                _textButtonClaim.SetText("today claimed");
            }
            
            _buttonClaim.interactable = isActive;
        }

        public void UpdateLog(string log, bool isSuccess)
        {
            _textLog.SetText(log);
            _textLog.color = isSuccess ? Color.green : Color.red;

            if (_clearLogsRoutine != null)
            {
                StopCoroutine(_clearLogsRoutine);
                _clearLogsRoutine = null;
            }
            
            _clearLogsRoutine = StartCoroutine(ClearLogsAsync());
        }

        public void ClearLogs()
        {
            _textLog.SetText("");
        }

        private IEnumerator ClearLogsAsync()
        {
            yield return _clearLogsDelay;
            
            ClearLogs();
            _clearLogsRoutine = null;
        }
    }
}