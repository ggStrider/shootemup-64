using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemy;
using Internal.Core.Pools;
using Internal.Gameplay;
using Tools;
using UnityEngine;
using Zenject;

namespace Shoot
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float _bulletSpeed = 25f;
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private float _lifeTime = 3f;

        [Space] [SerializeField] private float _subtractPointsOnFakeEnemyKilled = 1.5f;

        private BulletPool _bulletPool;
        private CancellationTokenSource _lifeTimeCts;

        [Inject]
        private void Construct(BulletPool bulletPool)
        {
            _bulletPool = bulletPool;
        }

        public void Initialize(Vector2 flyDirection)
        {
            if (_rigidbody == null)
            {
                Debug.LogError($"[{GetType().Name}] Rigidbody of bullet is null");
                return;
            }

            _rigidbody.linearVelocity = flyDirection * _bulletSpeed;

            Tools.UniTaskExtensions.SafeCancelAndCleanToken(ref _lifeTimeCts,
                createNewTokenAfter: true);

            CountLifeTime(_lifeTimeCts.Token).Forget();
        }

        private async UniTask CountLifeTime(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime),
                cancellationToken: _lifeTimeCts.Token);

            if (token.IsCancellationRequested || !gameObject.activeInHierarchy) return;
            _bulletPool.Despawn(this);
        }

        private void OnDisable()
        {
            Tools.UniTaskExtensions.SafeCancelAndCleanToken(ref _lifeTimeCts);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(StaticKeys.FAKE_ENEMY_TAG))
            {
                GameTimer.Instance.SubtractCurrentTime(_subtractPointsOnFakeEnemyKilled);
            }

            if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
            {
                enemy.OnDie();
            }

            Camera.main.DOShakePosition(0.1f, 0.25f);

            ToolsForEasyDebug.Destroy(other.gameObject);
            _bulletPool.Despawn(this);
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            _rigidbody ??= GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
        }
        #endif
    }
}