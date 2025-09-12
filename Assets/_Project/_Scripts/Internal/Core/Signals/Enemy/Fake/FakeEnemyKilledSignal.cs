using Enemy;

namespace Internal.Core.Signals
{
    public class FakeEnemyKilledSignal
    {
        public FakeEnemy FakeEnemy;

        public FakeEnemyKilledSignal(FakeEnemy fakeEnemy)
        {
            FakeEnemy = fakeEnemy;
        }
    }
}