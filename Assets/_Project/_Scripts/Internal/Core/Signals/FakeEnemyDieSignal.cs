using Enemy;

namespace Internal.Core.Signals
{
    public class FakeEnemyDieSignal
    {
        public FakeEnemy FakeEnemy;

        public FakeEnemyDieSignal(FakeEnemy fakeEnemy)
        {
            FakeEnemy = fakeEnemy;
        }
    }
}