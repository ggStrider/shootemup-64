using DG.Tweening;
using Enemy;
using Tools;
using UnityEngine;

namespace Shoot
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float _bulletSpeed = 25f;
        [SerializeField] private Rigidbody2D _rigidbody;

        [Space] [SerializeField] private float _subtractPointsOnFakeEnemyKilled = 1.5f;

        public void Initialize(Vector2 flyDirection)
        {
            if (_rigidbody == null)
            {
                Debug.LogError($"[{GetType().Name}] Rigidbody of bullet is null");
                return;
            }
            
            _rigidbody.linearVelocity = flyDirection * _bulletSpeed;
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
            ToolsForEasyDebug.Destroy(gameObject);
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