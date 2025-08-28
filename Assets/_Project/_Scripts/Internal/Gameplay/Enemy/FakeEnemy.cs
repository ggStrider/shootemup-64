using Internal.Core.Pools;
using Internal.Core.Signals;
using Shoot;
using Zenject;

namespace Enemy
{
    public class FakeEnemy : EnemyBase
    {
        private FakeEnemyPool _fakeEnemyPool;
        
        [Inject]
        private void Construct(FakeEnemyPool fakeEnemyPool)
        {
            _fakeEnemyPool = fakeEnemyPool;
        }

        protected override void OnDie()
        {
            SignalBus.Fire(new FakeEnemyDieSignal(this));
            InvokeAllIOnDestroy();
            DespawnSelf();
        }

        protected override void DespawnSelf()
        {
            if (!gameObject.activeInHierarchy) return;
            _fakeEnemyPool.Despawn(this);
        }
    }
}