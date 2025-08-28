using Enemy;
using Zenject;

namespace Internal.Core.Pools
{
    public class FakeEnemyPool : MonoMemoryPool<FakeEnemy>
    {
        protected override void OnSpawned(FakeEnemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        protected override void OnDespawned(FakeEnemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}