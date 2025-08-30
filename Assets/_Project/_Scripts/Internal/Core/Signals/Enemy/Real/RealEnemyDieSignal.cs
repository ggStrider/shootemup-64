using Enemy;

namespace Internal.Core.Signals
{
    public class RealEnemyDieSignal
    {
        public EnemyBase Enemy;

        public RealEnemyDieSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}