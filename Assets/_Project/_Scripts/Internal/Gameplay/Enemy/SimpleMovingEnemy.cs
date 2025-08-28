using Internal.Gameplay.EntitiesShared;
using UnityEngine;

namespace Enemy
{
    public class SimpleMovingEnemy : EnemyBase
    {
        [SerializeField, Min(1)] private int _damage;
        
        protected override void OnHitInPlayer(GameObject player)
        {
            if (player.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(_damage);
            }
            base.OnHitInPlayer(player);
        }
    }
}