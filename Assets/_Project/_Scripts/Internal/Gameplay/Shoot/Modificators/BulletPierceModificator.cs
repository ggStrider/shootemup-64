namespace Shoot
{
    public class BulletPierceModificator : IBulletModificator
    {
        public void ApplyModificator(BulletBehaviour bullet)
        {
            bullet.DespawnOnHit = false;
        }
    }
}