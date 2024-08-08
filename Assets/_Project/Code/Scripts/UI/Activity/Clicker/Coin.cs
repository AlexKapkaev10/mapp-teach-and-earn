using System;
using _Project.Scripts.Configs;
using _Project.Scripts.ObjectPoll;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI
{
    public class Coin : MonoBehaviour, IPointerClickHandler, IClickerItem
    {
        public event Action ClickItem;
        [SerializeField] private ClickerItemConfig _config;

        private PoolMono<FlyItem> _pool;
        private Transform _transform = default;
        private Tweener _tweener = default;
        private Vector2 _flyItemOffset = default;
        
        private float _endScaleValue = default;
        private float _endScaleDuration = default;

        private void Awake()
        {
            _transform = transform;
            _pool = new PoolMono<FlyItem>(_config.FlyItemPrefab, _config.PoolCount, _transform)
            {
                AutoExpand = _config.AutoExpandPool
            };

            _flyItemOffset = _config.GetFlyItemOffset();
            _endScaleValue = _config.EndScaleValue;
            _endScaleDuration = _config.EndScaleDuration;
        }
        
        public void ClickAnimation()
        {
            if (_tweener != null)
            {
                return;
            }

            _tweener = _transform
                .DOScale(_endScaleValue, _endScaleDuration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .OnComplete(() => _tweener = null);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickItem?.Invoke();
            IAnimateItem flyEffect = _pool.GetFreeElement();
            flyEffect.PlayAnimation(eventData.position + _flyItemOffset);
        }
    }
}