using Shoot;
using UnityEngine;

namespace Definitions.BulletModificators.Scripts
{
    [CreateAssetMenu(fileName = "Spawn Game Object Modificator", menuName = 
        StaticKeys.PROJECT_NAME + "/Bullet/Spawn Game Object Modificator")]
    public class SpawnGameObjectOnHitModificator : BulletModificatorSO
    {
        [SerializeField] private GameObject _prefabToSpawn;
        
        public override void ApplyModificator(BulletBehaviour bullet)
        {
            var modificator = new BulletSpawnGameObjectOnHitModificator(_prefabToSpawn);
            modificator.ApplyModificator(bullet);
        }
    }
}