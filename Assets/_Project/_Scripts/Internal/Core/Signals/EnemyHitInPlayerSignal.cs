using Enemy;

namespace Internal.Core.Signals
{
    public class EnemyHitInPlayerSignal
    {
        public EnemyBase Enemy;

        public EnemyHitInPlayerSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}