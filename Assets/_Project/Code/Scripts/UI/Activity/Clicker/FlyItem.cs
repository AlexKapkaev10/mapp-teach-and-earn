using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI
{
    public interface IAnimateItem
    {
        void PlayAnimation(Vector2 fromPosition);
        void SetHeader(string header);
    }
    
    public class FlyItem : MonoBehaviour, IAnimateItem
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private float _flyValue = 100f;
        [SerializeField] private float _flyDuration = 1.2f;

        public void SetHeader(string header)
        {
            _textHeader.SetText(header);
        }
        
        public void PlayAnimation(Vector2 fromPosition)
        {
            _rectTransform.position = fromPosition;
            
            _rectTransform.DOMoveY(_flyValue, _flyDuration)
                .SetEase(Ease.Linear)
                .SetRelative(true);
            
            _canvasGroup.DOFade(0, _flyDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    _canvasGroup.alpha = 1f;
                });
        }

        private void OnDisable()
        {
            DOTween.Kill(_rectTransform);
            DOTween.Kill(_canvasGroup);
        }
    }
}