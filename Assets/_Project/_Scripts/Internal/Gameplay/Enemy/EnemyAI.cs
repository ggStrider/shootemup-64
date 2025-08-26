using Audio;
using Internal.Core.Pools;
using Player;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Vector2 _directionToPlayer;
        [SerializeField] private float _speed = 10;

        [Space] [SerializeField, Min(0)] private int _damage = 1;

        private EnemyPool _enemyPool;
        private AudioEffectsManager _audioEffectsManager;

        [Inject]
        private void Construct(EnemyPool enemyPool, AudioEffectsManager audioEffectsManager)
        {
            _enemyPool = enemyPool;
            _audioEffectsManager = audioEffectsManager;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: Change Fake Enemy Tag into smth else. Like another 'Enemy' script
            if (!CompareTag(StaticKeys.FAKE_ENEMY_TAG))
            {
                if (other.CompareTag(StaticKeys.PLAYER_TAG))
                {
                    if (other.TryGetComponent<HealthSystem>(out var playerHealth))
                    {
                        playerHealth.Damage(_damage);
                    }
                    
                    // Use signals, or idk, instead of dependency of manager
                    _audioEffectsManager.PlayEnemyHitSound();
                }
            }

            DespawnSelf();
        }
    }
}
