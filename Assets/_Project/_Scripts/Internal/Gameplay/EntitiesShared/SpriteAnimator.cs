using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Internal.Gameplay.EntitiesShared
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private Sprite[] _frames;
        [SerializeField] private SpriteRenderer _animationTarget;

        [Space]
        [SerializeField, Min(1)] private int _fps = 12;
        [SerializeField] private bool _repeat;
        [SerializeField] private bool _startAnimationOnEnable;
        [SerializeField] private bool _disableSpriteRendererOnAnimationEnded;

        private int _currentFrame;
        private float _timeToNextFrame;
        private CancellationTokenSource _animationCts;

        protected virtual void OnEnable()
        {
            if (_startAnimationOnEnable)
            {
                SetupAndStartAnimation();
            }
        }

        protected virtual void OnDisable()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _animationCts);
            _currentFrame = 0;
        }

        protected void SetupAndStartAnimation()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _animationCts,
                createNewTokenAfter: true);
            
            _animationTarget.enabled = true;
            _timeToNextFrame = 1f / _fps;
            _currentFrame = 0;
            
            if (_timeToNextFrame <= 0)
            {
                Debug.LogError($"[{GetType().Name}] Time to Next Frame cannot be <= 0");
                return;
            }
            
            Animate().Forget();
        }

        private async UniTaskVoid Animate()
        {
            try
            {
                while (!_animationCts.IsCancellationRequested && _currentFrame < _frames.Length)
                {
                    _animationTarget.sprite = _frames[_currentFrame];
                    await UniTask.WaitForSeconds(_timeToNextFrame, cancellationToken: _animationCts.Token);
                    _currentFrame++;

                    if (_repeat)
                    {
                        if (_currentFrame >= _frames.Length)
                        {
                            _currentFrame = 0;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_disableSpriteRendererOnAnimationEnded)
                {
                    _animationTarget.enabled = false;
                }
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (_animationTarget == null)
            {
                if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                {
                    _animationTarget = spriteRenderer;
                }
            }
        }

        [Button]
        private void AnimateInEditor()
        {
            _currentFrame = 0;
            SetupAndStartAnimation();
        }

        [Button]
        private void StopAnimateInEditor()
        {
            OnDisable();
        }

        private void OnValidate()
        {
            _timeToNextFrame = 1f / _fps;
            if (_timeToNextFrame <= 0)
            {
                Debug.LogError($"[{GetType().Name}] Time to Next Frame cannot be <= 0");
                StopAnimateInEditor();
            }
        }
#endif
    }
}