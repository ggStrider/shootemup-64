using Internal.Core.Pools;
using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class SimpleMovingEnemy : EnemyBase
    {
        [SerializeField, Min(1)] private int _damage;
        private SimpleEnemyPool _simpleEnemyPool;

        [Inject]
        private void Construct(SimpleEnemyPool simpleEnemyPool)
        {
            _simpleEnemyPool = simpleEnemyPool;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SignalBus.Subscribe<BackgroundChangedSignal>(ChangeSkinColor);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SignalBus.TryUnsubscribe<BackgroundChangedSignal>(ChangeSkinColor);
        }
        
        protected override void OnHitInPlayer(GameObject player)
        {
            if (player.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(_damage);
            }
            
            base.OnHitInPlayer(player);
            SignalBus.Fire(new RealEnemyHitInPlayerSignal(this));
        }
        
        protected override void DespawnSelf()
        {
            if (!gameObject.activeInHierarchy) return;
            _simpleEnemyPool.Despawn(this);
        }
    }
}