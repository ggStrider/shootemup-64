using Enemy;
using Zenject;

namespace Internal.Core.Pools
{
    public class EnemyPool : MonoMemoryPool<EnemyBase>
    {
        protected override void OnSpawned(EnemyBase enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        protected override void OnDespawned(EnemyBase enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}