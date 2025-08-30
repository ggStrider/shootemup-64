using Enemy;

namespace Internal.Core.Signals
{
    public class AnyEnemyHitInPlayerSignal
    {
        public EnemyBase Enemy;

        public AnyEnemyHitInPlayerSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}