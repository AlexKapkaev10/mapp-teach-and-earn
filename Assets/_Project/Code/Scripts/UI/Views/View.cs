using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class View : MonoBehaviour
    {
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
            if (_ignoreSetVisible)
            {
                Destroy(gameObject);
                return;
            }
            
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
}