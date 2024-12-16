using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI.Items
{
    public class LoaderItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _rotateTransform;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private Vector3 _rotationAxis = new (0, 0, -360);
        
        private Tween _rotationTween;

        private void OnEnable()
        {
            _rotationTween = _rotateTransform.DORotate(_rotationAxis, _duration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .Pause()
                .SetAutoKill(false);
            
            StartRotation();
        }

        private void StartRotation()
        {
            if (_rotationTween.IsActive() && !_rotationTween.IsPlaying())
            {
                _rotationTween.Play();
            }
        }

        private void KillRotation()
        {
            if (_rotationTween.IsActive())
            {
                _rotationTween.Kill();
            }
        }

        private void OnDisable()
        {
            KillRotation();
        }
    }
}