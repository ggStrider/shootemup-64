using Shoot;
using Zenject;

namespace Internal.Core.Pools
{
    public class BulletPool : MonoMemoryPool<BulletBehaviour>
    {
        protected override void OnSpawned(BulletBehaviour bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        protected override void OnDespawned(BulletBehaviour bullet)
        {
            bullet.gameObject.SetActive(false);
        }
    }
}