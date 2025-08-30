using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using NaughtyAttributes;
using Shoot;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IHittableByBullet
    {
        [SerializeField, ReadOnly] private Vector2 _directionToPlayer;
        [SerializeField] protected float Speed = 10;

        [Space] [SerializeField] protected EnemyHealth Health;
        [SerializeField] protected SpriteRenderer SpriteRenderer;

        public float SpeedMultiplier { get; set; } = 1;

        protected SignalBus SignalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }

        protected virtual void OnEnable()
        {
            if (Health) Health.OnDeath += OnDie;
        }

        protected virtual void OnDisable()
        {
            if (Health) Health.OnDeath -= OnDie;
        }
        
        public void ChangeSkinColor(BackgroundChangedSignal newData)
        {
            ChangeSkinColor(newData.NewColor);
        }

        public void ChangeSkinColor(Color newColor)
        {
            SpriteRenderer.color = newColor;
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
            SignalBus.Fire(new EnemyDieSignal(this));
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

        protected abstract void DespawnSelf();

        private void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            // transform is body of this enemy
            transform.Translate(_directionToPlayer * 
                                (Speed * Time.fixedDeltaTime * SpeedMultiplier));
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(StaticKeys.PLAYER_TAG))
            {
                SignalBus.Fire(new AnyEnemyHitInPlayerSignal(this));
                OnHitInPlayer(other.gameObject);
            }
        }

        protected virtual void OnHitInPlayer(GameObject player)
        {
            DespawnSelf();
        }

        public virtual void OnHitByBullet(BulletBehaviour bulletWhichHit)
        {
            Health.TakeDamage(bulletWhichHit.Damage);
        }
    }
}