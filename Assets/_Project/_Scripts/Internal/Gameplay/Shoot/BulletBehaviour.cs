using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Core.Pools;
using NaughtyAttributes;
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

        [field: Space, SerializeField] public int Damage { get; private set; } = 1;
        
        // Applies by modificators
        [field: SerializeField, ReadOnly] public bool DespawnOnHit { get; set; } = true;
        
        public event Action<BulletBehaviour, GameObject> OnHitInCollision;
        public event Action<BulletBehaviour> OnDespawn;

        private Vector3 _flyDirection;
        private BulletPool _bulletPool;
        private CancellationTokenSource _lifeTimeCts;

        [Inject]
        private void Construct(BulletPool bulletPool)
        {
            _bulletPool = bulletPool;
        }

        public void InitializeBeforeShoot(Vector2 flyDirection)
        {
            if (_rigidbody == null)
            {
                Debug.LogError($"[{GetType().Name}] Rigidbody of bullet is null");
                return;
            }

            _flyDirection = flyDirection;
            SetVelocity();

            Tools.MyUniTaskExtensions.SafeCancelAndCleanToken(ref _lifeTimeCts,
                createNewTokenAfter: true);

            CountLifeTime().Forget();
        }
        
        private void SetVelocity() => _rigidbody.linearVelocity = _flyDirection * _bulletSpeed;

        private async UniTask CountLifeTime()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime), cancellationToken: _lifeTimeCts.Token);
                DespawnSelf();
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private void DespawnSelf()
        {
            // already despawned
            if (!gameObject.activeSelf) return;

            OnDespawn?.Invoke(this);
            _bulletPool.Despawn(this);
        }

        private void OnDisable()
        {
            Tools.MyUniTaskExtensions.SafeCancelAndCleanToken(ref _lifeTimeCts);
            ResetModificators();
        }

        private void ResetModificators()
        {
            DespawnOnHit = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var allHittable = other.gameObject.GetComponents<IHittableByBullet>();
            if (allHittable.Length > 0)
            {
                OnHitInCollision?.Invoke(this, other.gameObject);
                foreach (var hittable in allHittable)
                {
                    hittable?.OnHitByBullet(this);
                }
            }

            if (DespawnOnHit)
            {
                DespawnSelf();
            }
            else
            {
                // Because when hits rigidbody - velocity becomes zero
                SetVelocity();
            }
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