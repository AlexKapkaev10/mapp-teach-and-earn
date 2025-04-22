using UnityEngine;
using DG.Tweening;

namespace Project.Scripts.Tools
{
    public class BezierMover : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _easeType = Ease.Linear;
        [SerializeField] private int _pathResolution = 10;
        [SerializeField] private bool _lookAtPath = false;
        [SerializeField] private bool _autoStart = false;

        private Vector3[] _controlPoints;
        private Sequence _currentSequence;

        private void Start()
        {
            if (_autoStart && _controlPoints != null && _controlPoints.Length >= 4)
            {
                StartMovement();
            }
        }

        public void SetControlPoints(Vector3[] points)
        {
            if (points == null || points.Length < 4)
            {
                Debug.LogError("BezierMover: Need at least 4 control points for a cubic Bezier curve");
                return;
            }

            _controlPoints = points;
        }

        public void StartMovement()
        {
            if (_target == null)
            {
                Debug.LogError("BezierMover: Target transform is not set");
                return;
            }

            if (_controlPoints == null || _controlPoints.Length < 4)
            {
                Debug.LogError("BezierMover: Control points are not set");
                return;
            }

            // Kill any existing movement
            if (_currentSequence != null)
            {
                _currentSequence.Kill();
            }

            // Create path points using DOTween's CubicBezier
            Vector3[] pathPoints = DOCurve.CubicBezier.GetSegmentPointCloud(
                _controlPoints[0],
                _controlPoints[1],
                _controlPoints[2],
                _controlPoints[3],
                _pathResolution
            );

            // Create the movement sequence
            _currentSequence = DOTween.Sequence();

            // Add the path movement
            _currentSequence.Append(_target.DOPath(pathPoints, _duration, PathType.CubicBezier)
                .SetEase(_easeType));

            // Add look at path if enabled
            if (_lookAtPath)
            {
                _currentSequence.Join(_target.DOLookAt(pathPoints[1], _duration / pathPoints.Length)
                    .SetEase(_easeType));
            }
        }

        public void StopMovement()
        {
            if (_currentSequence != null)
            {
                _currentSequence.Kill();
                _currentSequence = null;
            }
        }

        private void OnDestroy()
        {
            StopMovement();
        }
    }
}