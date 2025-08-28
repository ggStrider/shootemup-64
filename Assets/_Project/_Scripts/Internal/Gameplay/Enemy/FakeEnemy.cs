using Shoot;

namespace Enemy
{
    public class FakeEnemy : EnemyBase
    {
        public override void OnHitByBullet(BulletBehaviour bulletWhichHit)
        {
            base.OnHitByBullet(bulletWhichHit);
        }
    }
}