using Enemy;
using Shoot;
using Zenject;

namespace Internal.Core.Pools
{
    public class EnemyPool : MonoMemoryPool<EnemyAI>
    {
        protected override void OnSpawned(EnemyAI enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        protected override void OnDespawned(EnemyAI enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}