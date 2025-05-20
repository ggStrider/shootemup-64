using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Images
{
    public class TransitionImageMover : MonoBehaviour
    {
        [SerializeField] private RectTransform _toMove;
        [SerializeField] private float _duration = 1.7f;

        [Space] [SerializeField] private Vector3 _hiddenOnLeftPosition;
        [SerializeField] private Vector3 _hiddenOnRightPosition;
        [SerializeField] private Vector3 _overlayScreenPosition;

        [Space] [SerializeField] private bool _moveOnStart;
        [ShowIf(nameof(_moveOnStart))] [SerializeField] private MoveToTypes _moveToOnStart;
        
        public static TransitionImageMover Instance { get; private set; }

        public event Action OnOverlayed;
        
        private Tween _currentTween;

        public enum MoveToTypes
        {
            ToLeft,
            ToRight,
            OverlayScreen
        }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            if (_moveOnStart)
            {
                _toMove.anchoredPosition = _overlayScreenPosition;
                MoveTo(_moveToOnStart);
            }
        }

        public void MoveTo(MoveToTypes side)
        {
            _currentTween?.Kill();
            switch (side)
            {
                case MoveToTypes.ToLeft:
                    MoveToLeft();
                    break;
                case MoveToTypes.ToRight:
                    MoveToRight();
                    break;
                case MoveToTypes.OverlayScreen:
                    MoveToOverlay();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private void MoveToLeft()
        {
            _currentTween = _toMove.DOLocalMove(_hiddenOnLeftPosition, _duration);
        }

        private void MoveToRight()
        {
            _currentTween = _toMove.DOLocalMove(_hiddenOnRightPosition, _duration);
        }

        private void MoveToOverlay()
        {
            _currentTween = _toMove.DOLocalMove(_overlayScreenPosition, _duration)
                .OnComplete(() => {
                    OnOverlayed?.Invoke();
                });
        }

        private void OnDestroy()
        {
            OnOverlayed = null;
        }

        #region FOR EDITOR BUTTONS

        [NaughtyAttributes.Button]
        private void WriteHiddenOnLeft()
        {
            _hiddenOnLeftPosition = _toMove.anchoredPosition;
        }

        [NaughtyAttributes.Button]
        private void WriteHiddenOnRight()
        {
            _hiddenOnRightPosition = _toMove.anchoredPosition;
        }

        [NaughtyAttributes.Button]
        private void WriteOverlayScreenOnLeft()
        {
            _overlayScreenPosition = _toMove.anchoredPosition;
        }

        [NaughtyAttributes.Button]
        private void SetToHiddenOnLeft()
        {
            _toMove.anchoredPosition = _hiddenOnLeftPosition;
        }

        [NaughtyAttributes.Button]
        private void SetToHiddenOnRight()
        {
            _toMove.anchoredPosition = _hiddenOnRightPosition;
        }

        [NaughtyAttributes.Button]
        private void SetToOverlayScreenOnLeft()
        {
            _toMove.anchoredPosition = _overlayScreenPosition;
        }

        #endregion
    }
}