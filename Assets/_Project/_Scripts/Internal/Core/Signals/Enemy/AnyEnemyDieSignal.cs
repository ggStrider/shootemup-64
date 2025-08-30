using Enemy;

namespace Internal.Core.Signals
{
    public class AnyEnemyDieSignal
    {
        public EnemyBase Enemy;

        public AnyEnemyDieSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}