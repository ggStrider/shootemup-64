using Shoot;
using UnityEngine;

namespace Internal.Core.Signals
{
    public class PlayerShootSignal
    {
        public BulletBehaviour Bullet;
        public Vector2Int Direction;

        public PlayerShootSignal(BulletBehaviour bullet, Vector2Int direction)
        {
            Bullet = bullet;
            Direction = direction;
        }
    }
}