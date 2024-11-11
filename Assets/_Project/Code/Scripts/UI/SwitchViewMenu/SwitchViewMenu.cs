using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public interface ISwitchViewMenu
    {
        
    }
    
    public class SwitchViewMenu : View, ISwitchViewMenu
    {
        [SerializeField] private ViewStateType _defaultType = ViewStateType.Home;

        private ISwitchViewItem[] _switchViewItems;
        private IViewsStateMachine _viewsStateMachine;
        private ISwitchViewItem _currentItem;
        
        [Inject]
        private void Construct(IViewsStateMachine viewsStateMachine)
        {
            _viewsStateMachine = viewsStateMachine;
        }

        private void Awake()
        {
            _switchViewItems = GetComponentsInChildren<ISwitchViewItem>();

            foreach (var item in _switchViewItems)
            {
                item.SwitchView += ItemOnSwitchView;
                item.SetActive(false);
                if (item.Type == _defaultType)
                {
                    _currentItem = item;
                }
            }
        }

        private void Start()
        {
            if (_currentItem != null)
            {
                ItemOnSwitchView(_currentItem);
            }
            
            _currentItem.SetActive(true);
        }

        private void ItemOnSwitchView(ISwitchViewItem item)
        {
            _currentItem.SetActive(false);
            _currentItem = item;
            _currentItem.SetActive(true);
            _viewsStateMachine.SwitchStateByType(item.Type);
        }
    }
}