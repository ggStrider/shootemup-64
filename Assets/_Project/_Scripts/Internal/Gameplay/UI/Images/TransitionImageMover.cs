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

        private Tween _currentTween;

        private bool _isStartTransition;
        public event Action StartTransitionEnded;

        public enum MoveToTypes
        {
            ToLeft,
            ToRight,
            OverlayScreen
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }   
        }

        private void Start()
        {
            if (_moveOnStart)
            {
                _toMove.anchoredPosition = _overlayScreenPosition;
                _isStartTransition = true;
                MoveTo(_moveToOnStart);
            }
        }

        public void MoveTo(MoveToTypes side, Action onMoved = null)
        {
            _currentTween?.Kill();
            switch (side)
            {
                case MoveToTypes.ToLeft:
                    MoveToLeft(onMoved);
                    break;
                case MoveToTypes.ToRight:
                    MoveToRight(onMoved);
                    break;
                case MoveToTypes.OverlayScreen:
                    MoveToOverlay(onMoved);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private void MoveToLeft(Action onMoved = null)
        {
            _currentTween = _toMove.DOLocalMove(_hiddenOnLeftPosition, _duration)
                .OnComplete(() => {
                    onMoved?.Invoke();
                    IfStartInvokeAction();
                });;
        }

        private void MoveToRight(Action onMoved = null)
        {
            _currentTween = _toMove.DOLocalMove(_hiddenOnRightPosition, _duration)
                .OnComplete(() => {
                    onMoved?.Invoke();
                    IfStartInvokeAction();
                });;
        }

        private void MoveToOverlay(Action onMoved = null)
        {
            _currentTween = _toMove.DOLocalMove(_overlayScreenPosition, _duration)
                .OnComplete(() => {
                    onMoved?.Invoke();
                    IfStartInvokeAction();
                });
        }

        private void IfStartInvokeAction()
        {
            if (_isStartTransition)
            {
                _isStartTransition = false;
                StartTransitionEnded?.Invoke();
                StartTransitionEnded = null;
            }
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