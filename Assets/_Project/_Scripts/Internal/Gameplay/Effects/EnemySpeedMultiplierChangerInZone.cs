using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemy;
using Tools;
using UnityEngine;

namespace Internal.Gameplay.Effects
{
    public class EnemySpeedMultiplierChangerInZone : MonoBehaviour
    {
        [SerializeField] private float _newSpeedMultiplier = 1.5f;
        [SerializeField] private float _lifeTime = 1.5f;
        [SerializeField] private float _tweenDuration = 0.3f;

        private Vector3 _originalSize;
        private CancellationTokenSource _lifeTimeCts;
        private readonly HashSet<EnemyBase> _enemiesInZone = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBase>(out var enemy))
            {
                _enemiesInZone.Add(enemy);
                enemy.SpeedMultiplier = _newSpeedMultiplier;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBase>(out var enemy))
            {
                _enemiesInZone.Remove(enemy);

                // no other effect was applied
                if (Mathf.Approximately(enemy.SpeedMultiplier, _newSpeedMultiplier))
                {
                    enemy.SpeedMultiplier = 1;
                }
            }
        }

        private void Awake()
        {
            _originalSize = transform.localScale;
        }
        
        private void OnEnable()
        {
            _lifeTimeCts = new();
            transform.localScale = Vector3.zero;
            ChangeSize(_originalSize);
            CountLifeTime().Forget();
        }

        private void ChangeSize(Vector3 size)
        {
            transform.DOScale(size, _tweenDuration);
        }

        private async UniTaskVoid CountLifeTime()
        {
            try
            {
                await UniTask.WaitForSeconds(_lifeTime - _tweenDuration, cancellationToken: _lifeTimeCts.Token);
                ChangeSize(Vector3.zero);
                await UniTask.WaitForSeconds(_tweenDuration, cancellationToken: _lifeTimeCts.Token);
                
                Destroy(gameObject);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in _enemiesInZone)
            {
                enemy.SpeedMultiplier = 1;
            }

            _enemiesInZone.Clear();
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _lifeTimeCts);
        }
    }
}