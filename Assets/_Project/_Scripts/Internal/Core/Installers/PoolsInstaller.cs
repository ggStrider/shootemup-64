using Zenject;
using UnityEngine;
using Shoot;
using Internal.Core.Pools;
using Enemy;

namespace Internal.Core.Installers
{
    public class PoolsInstaller : MonoInstaller
    {
        [SerializeField] private BulletBehaviour _bulletPrefab;
        
        [Space]
        [SerializeField] private SimpleMovingEnemy _simpleMoveEnemy;
        [SerializeField] private FakeEnemy _fakeEnemy;

        private Transform _poolParent;

        private const int BULLET_POOL_START_SIZE = 40;
        private const int ENEMY_POOL_START_SIZE = 10;

        public override void InstallBindings()
        {
            if (!_poolParent) _poolParent = CreateParentForPool("ParentForPoolObjects", false);

            Container.BindMemoryPool<BulletBehaviour, BulletPool>()
                .WithInitialSize(BULLET_POOL_START_SIZE)
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransform(CreateParentForPool("(pool)Bullets"))
                .AsCached();

            Container.BindMemoryPool<SimpleMovingEnemy, SimpleEnemyPool>()
                .WithInitialSize(ENEMY_POOL_START_SIZE)
                .FromComponentInNewPrefab(_simpleMoveEnemy)
                .UnderTransform(CreateParentForPool("(pool)Simple Enemies"))
                .AsCached();
            
            Container.BindMemoryPool<FakeEnemy, FakeEnemyPool>()
                .WithInitialSize(ENEMY_POOL_START_SIZE)
                .FromComponentInNewPrefab(_fakeEnemy)
                .UnderTransform(CreateParentForPool("(pool)Fake Enemies"))
                .AsCached();
        }

        private Transform CreateParentForPool(string poolName, bool underParent = true)
        {
            var go = new GameObject
            {
                name = poolName
            };

            if(underParent) go.transform.SetParent(_poolParent);
            go.transform.position = Vector3.zero;

            return go.transform;
        }
    }
}