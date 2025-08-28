using Enemy;
using Zenject;

namespace Internal.Core.Pools
{
    public class SimpleEnemyPool : MonoMemoryPool<SimpleMovingEnemy>
    {
        protected override void OnSpawned(SimpleMovingEnemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        protected override void OnDespawned(SimpleMovingEnemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}