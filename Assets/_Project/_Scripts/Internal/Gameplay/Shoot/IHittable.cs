namespace Shoot
{
    public interface IHittable
    {
        public void OnHit(BulletBehaviour bulletWhichHit);
    }
}