namespace Shoot
{
    public interface IHittableByBullet
    {
        public void OnHitByBullet(BulletBehaviour bulletWhichHit);
    }
}