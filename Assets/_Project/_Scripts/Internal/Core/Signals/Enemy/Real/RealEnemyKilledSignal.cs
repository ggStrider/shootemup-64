using Enemy;

namespace Internal.Core.Signals
{
    public class RealEnemyKilledSignal
    {
        public EnemyBase Enemy;

        public RealEnemyKilledSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}