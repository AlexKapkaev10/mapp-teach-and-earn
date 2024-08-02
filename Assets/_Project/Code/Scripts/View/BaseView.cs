using DG.Tweening;
using NewRP.Game.Infrastructure.Extensions;
using UnityEngine;

namespace Project.Code.Scripts.View
{
    public class BaseView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        protected virtual void OnEnable()
        {
            SetVisible(true);
        }

        protected virtual void OnDestroy()
        {
            DOTween.Kill(_canvasGroup);
        }

        private void SetVisible(bool isVisible)
        {
            _canvasGroup.AnimationDisplayFrom(isVisible, 0);
        }
    }
}