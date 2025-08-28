using Enemy;

namespace Internal.Core.Signals
{
    public class EnemyDieSignal
    {
        public EnemyBase Enemy;

        public EnemyDieSignal(EnemyBase enemy)
        {
            Enemy = enemy;
        }
    }
}