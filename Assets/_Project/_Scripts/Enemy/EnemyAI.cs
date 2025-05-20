using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Vector2 _directionToPlayer;
        [SerializeField] private float _speed = 10;
        
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

        private void FixedUpdate()
        {
            transform.Translate(_directionToPlayer * (_speed * Time.fixedDeltaTime));
        }
    }
}
