using System.Collections.Generic;
using Project.Scripts.UI.Activity;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Scripts.UI
{
    public sealed class ActivityView : View
    {
        [SerializeField] private ActivityViewConfig _config;
        [SerializeField] private ScrollRect _activityScrollRect;
        private readonly List<IActivityButton> _activityButtons = new List<IActivityButton>();
        private IViewsStateMachine _viewsStateMachine = default;

        [Inject]
        private void Construct(IViewsStateMachine stateMachine)
        {
            _viewsStateMachine = stateMachine;
        }

        private void Awake()
        {
            for (var i = 0; i < _config._activityButtonsData.Length; i++)
            {
                var data = _config._activityButtonsData[i];
                var button = Instantiate(_config._buttonPrefab, _activityScrollRect.content);
                button.SetData(data);
                _activityButtons.Add(button);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            foreach (var activityButton in _activityButtons)
            {
                activityButton.Click += OnClickActivityButton;
            }
        }

        private void OnDisable()
        {
            foreach (var activityButton in _activityButtons)
            {
                activityButton.Click -= OnClickActivityButton;
            }
        }

        private void OnClickActivityButton(ViewStateType type)
        {
            _viewsStateMachine.SwitchViewByType(type);
        }
    }
}