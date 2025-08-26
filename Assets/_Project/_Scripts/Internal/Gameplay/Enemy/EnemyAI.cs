using Internal.Core.Pools;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Vector2 _directionToPlayer;
        [SerializeField] private float _speed = 10;

        private EnemyPool _enemyPool;

        [Inject]
        private void Construct(EnemyPool enemyPool)
        {
            _enemyPool = enemyPool;
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
        }

        public void OnDie()
        {
            var onDestroy = GetComponents<IOnEnemyDestroy>();
            foreach (var onEnemyDestroy in onDestroy)
            {
                onEnemyDestroy?.OnEnemyDestroy();
            }
        }

        public void DespawnSelf()
        {
            if (!gameObject.activeSelf) return;
            _enemyPool.Despawn(this);
        }

        private void FixedUpdate()
        {
            transform.Translate(_directionToPlayer * (_speed * Time.fixedDeltaTime));
        }
    }
}
