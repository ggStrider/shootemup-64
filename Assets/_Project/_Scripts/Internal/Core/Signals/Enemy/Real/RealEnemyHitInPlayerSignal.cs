using Enemy;

namespace Internal.Core.Signals
{
    public class RealEnemyHitInPlayerSignal
    {
        public SimpleMovingEnemy RealEnemy;

        public RealEnemyHitInPlayerSignal(SimpleMovingEnemy enemy)
        {
            RealEnemy = enemy;
        }
    }
}