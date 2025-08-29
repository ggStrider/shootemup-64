using Shoot;

namespace Internal.Core.Signals
{
    public class PlayerShootSignal
    {
        public BulletBehaviour Bullet;

        public PlayerShootSignal(BulletBehaviour bullet)
        {
            Bullet = bullet;
        }
    }
}