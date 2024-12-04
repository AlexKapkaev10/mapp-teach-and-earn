using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class View : MonoBehaviour
    {
        [field: SerializeField] public ViewType ViewType { get; private set; }
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _durationVisible = 0.4f;
        [SerializeField] private bool _ignoreSetVisible = false;

        private Tween _tweener;

        protected virtual void OnEnable()
        {
            if (_ignoreSetVisible)
            {
                return;
            }

            _canvasGroup.alpha = 0f;
            SetEnable();
        }

        private void SetEnable()
        {
            _tweener = _canvasGroup.DOFade(1, _durationVisible)
                .From(0f)
                .SetEase(Ease.Linear)
                .OnComplete(()=> _tweener = null);
        }

        public virtual void SetDisable()
        {
            _tweener = _canvasGroup.DOFade(0, _durationVisible)
                .From(1f)
                .SetEase(Ease.Linear)
                .OnComplete(()=> Destroy(gameObject));
        }

        private void OnDestroy()
        {
            if (_tweener != null)
            {
                DOTween.Kill(_tweener);
            }
        }
    }

    public enum ViewType
    {
        None = 0,
        Main = 1,
        Character = 2,
        Clicker = 3,
        GameInfo = 4,
        Loader = 5,
        GradientBg = 6,
        Activity = 7,
        CheckFPS = 8,
        SwitchViewMenu = 9,
        Quest = 10
    }
}