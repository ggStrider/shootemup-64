using Enemy;

namespace Internal.Core.Signals
{
    public class FakeEnemyHitInPlayerSignal
    {
        public FakeEnemy FakeEnemy;

        public FakeEnemyHitInPlayerSignal(FakeEnemy fakeEnemy)
        {
            FakeEnemy = fakeEnemy;
        }
    }
}