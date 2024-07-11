using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameCharacter.Skin
{
    public interface ICharacterSkin
    {
        void Initialize(AnimationData data);
        void PlayClickAnimation();
        void StopLoopAnimation();
    }
    
    public sealed class CharacterSkin : MonoBehaviour, ICharacterSkin
    {
        [SerializeField] private Image _image;

        private Transform _transform = default;
        private AnimationData _animationData = default;
        private Tween _tweenBounce = default;

        public void Initialize(AnimationData data)
        {
            _transform = transform;
            
            _animationData = data;

            if (_animationData.PlayOnAwake)
            {
                PlayLoopAnimation();
            }
        }

        public void PlayClickAnimation()
        {
            if (_tweenBounce != null)
            {
                return;
            }
            
            StopLoopAnimation();
            
            _tweenBounce = _transform
                .DOScale(0.2f, 0.1f)
                .SetRelative(true)
                .From(1)
                .SetLoops(_animationData.ClickAnimationCount, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _tweenBounce = null;
                    PlayLoopAnimation();
                });
        }

        public void StopLoopAnimation()
        {
            DOTween.Kill(_transform);
            DOTween.Kill(_image);
        }

        private void OnDisable()
        {
            if (_tweenBounce != null)
            {
                DOTween.Kill(_tweenBounce);
            }
        }

        private void PlayLoopAnimation()
        {
            _transform.DOScale(_animationData.ScaleAnimationOffset, _animationData.ScaleAnimationTimeLoop)
                .SetRelative(true)
                .SetEase(_animationData.ScaleAnimationEase)
                .SetLoops(-1, LoopType.Yoyo);
                
            _image.DOColor(_animationData.ColorTo, _animationData.ScaleAnimationTimeLoop)
                .From(_animationData.ColorFrom)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    [Serializable]
    public struct AnimationData
    {
        public Color ColorFrom;
        public Color ColorTo;
        public Vector3 ScaleAnimationOffset;
        public Ease ScaleAnimationEase;
        public bool PlayOnAwake;
        public int ClickAnimationCount;
        public float ScaleAnimationTimeLoop;
    }
}