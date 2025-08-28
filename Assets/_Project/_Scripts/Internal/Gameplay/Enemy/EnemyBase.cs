using Internal.Core.Pools;
using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using Shoot;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IHittableByBullet
    {
        [SerializeField] private Vector2 _directionToPlayer;
        [SerializeField] protected float Speed = 10;

        [Space] [SerializeField] protected EnemyHealth Health;

        private EnemyPool _enemyPool;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(EnemyPool enemyPool, SignalBus signalBus)
        {
            _enemyPool = enemyPool;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            if (Health != null)
            {
                Health.OnDeath += OnDie;
            }
        }

        private void OnDisable()
        {
            if (Health != null)
            {
                Health.OnDeath -= OnDie;
            }
        }

        public void Initialize(Transform player)
        {
            _directionToPlayer = player.position - transform.position;
            if (Mathf.Abs(_directionToPlayer.x) > Mathf.Abs(_directionToPlayer.y))
            {
                _directionToPlayer.x = Mathf.Sign(_directionToPlayer.x);
            }
            else
            {
                _directionToPlayer.y = Mathf.Sign(_directionToPlayer.y);
            }
            
            Health.InitializeHealth();
        }

        protected virtual void OnDie()
        {
            _signalBus.Fire(new EnemyDieSignal(this));
            InvokeAllIOnDestroy();
            DespawnSelf();
        }

        protected void InvokeAllIOnDestroy()
        {
            var onDestroy = GetComponents<IOnEnemyDestroy>();
            foreach (var onEnemyDestroy in onDestroy)
            {
                onEnemyDestroy?.OnEnemyDestroy();
            }
        }

        private void DespawnSelf()
        {
            if (!gameObject.activeSelf) return;
            _enemyPool.Despawn(this);
        }

        private void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            // transform is body of this enemy
            transform.Translate(_directionToPlayer * (Speed * Time.fixedDeltaTime));
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(StaticKeys.PLAYER_TAG))
            {
                OnHitInPlayer(other.gameObject);
                _signalBus.Fire(new EnemyHitInPlayerSignal(this));
            }
        }

        protected virtual void OnHitInPlayer(GameObject player)
        {
            DespawnSelf();
            
            // TODO: Refactor to use new Enemies in pool
            if (gameObject.CompareTag(StaticKeys.FAKE_ENEMY_TAG)) return;
            if (player.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(1);
            }
        }

        public virtual void OnHitByBullet(BulletBehaviour bulletWhichHit)
        {
            Health.TakeDamage(bulletWhichHit.Damage);
        }
    }
}