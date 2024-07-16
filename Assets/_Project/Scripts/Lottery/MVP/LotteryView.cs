using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Lottery
{
    public interface ILotteryView
    {
        RectTransform LotteryItemParent { get; }
    }
    
    public class LotteryView : MonoBehaviour, ILotteryView
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _buttonAdd;
        [field: SerializeField] public RectTransform LotteryItemParent { get; private set; }

        private ILotteryPresenter _presenter;

        [Inject]
        public void Construct(ILotteryPresenter presenter)
        {
            _presenter = presenter;
            _presenter.SetView(this);
        }

        private void OnEnable()
        {
            _buttonAdd?.onClick.AddListener(ClickAdd);
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