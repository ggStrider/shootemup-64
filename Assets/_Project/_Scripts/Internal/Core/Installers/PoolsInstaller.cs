using Zenject;
using UnityEngine;
using Shoot;
using Internal.Core.Pools;

namespace Internal.Core.Installers
{
    public class PoolsInstaller : MonoInstaller
    {
        [SerializeField] private BulletBehaviour _bulletPrefab;

        [Space(10)]
        [SerializeField] private Transform _poolParent;

        private const int BULLET_POOL_START_SIZE = 100;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<BulletBehaviour, BulletPool>()
                .WithInitialSize(BULLET_POOL_START_SIZE)
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransform(CreateParentForPool("(pool)Bullets"))
                .AsSingle();
        }

        private Transform CreateParentForPool(string poolName)
        {
            var go = new GameObject
            {
                name = poolName
            };

            go.transform.SetParent(_poolParent);
            go.transform.position = Vector3.zero;

            return go.transform;
        }
    }
}