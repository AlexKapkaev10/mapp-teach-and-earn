using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Lottery
{
    public interface ILotteryView
    {
        RectTransform LotteryItemParent { get; }
        void SetActiveDeleteButton(bool isActive);
    }
    
    public class LotteryView : MonoBehaviour, ILotteryView
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _buttonAdd;
        [SerializeField] private Button _buttonClear;
        [field: SerializeField] public RectTransform LotteryItemParent { get; private set; }

        private ILotteryPresenter _presenter;

        [Inject]
        public void Construct(ILotteryPresenter presenter)
        {
            _presenter = presenter;
            _presenter.SetView(this);
        }

        private void Awake()
        {
            SetActiveDeleteButton(false);
        }

        public void SetActiveDeleteButton(bool isActive)
        {
            if (_buttonClear.gameObject.activeInHierarchy == isActive)
            {
                return;
            }
            
            _buttonClear.gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            _buttonAdd?.onClick.AddListener(ClickAdd);
            _buttonClear?.onClick.AddListener(ClickClear);
        }

        private void ClickClear()
        {
            _presenter.Clear();
        }

        private void OnDisable()
        {
            _buttonAdd?.onClick.RemoveListener(ClickAdd);
        }

        private void ClickAdd()
        {
            _presenter.AddItem(_inputField.text);
            _inputField.SetTextWithoutNotify(null);
        }
    }
}