using Enemy;

namespace Internal.Core.Signals
{
    public class AnyEnemyKilledSignal
    {
        public EnemyBase Enemy;

        public AnyEnemyKilledSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}