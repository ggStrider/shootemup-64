using UnityEngine;

namespace Shoot
{
    public class BulletSpawnGameObjectOnHitModificator : IBulletModificator
    {
        private readonly GameObject _prefabToSpawn;
        
        public BulletSpawnGameObjectOnHitModificator(GameObject prefabToSpawn)
        {
            _prefabToSpawn = prefabToSpawn;
        }
        
        public void ApplyModificator(BulletBehaviour bullet)
        {
            bullet.OnHitInCollision += SpawnGameObject;
            bullet.OnDespawn += UnsubscribeFromEvent;
        }

        private void UnsubscribeFromEvent(BulletBehaviour bullet)
        {
            bullet.OnHitInCollision -= SpawnGameObject;
            bullet.OnDespawn -= UnsubscribeFromEvent;
        }

        private void SpawnGameObject(BulletBehaviour bullet, GameObject collisionObject)
        {
            Object.Instantiate(_prefabToSpawn, bullet.transform.position, Quaternion.identity);
        }
    }
}