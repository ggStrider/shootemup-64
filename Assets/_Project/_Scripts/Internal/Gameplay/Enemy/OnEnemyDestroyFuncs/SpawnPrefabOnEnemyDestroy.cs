using UnityEngine;

namespace Enemy.OnEnemyDestroyFuncs
{
    public class SpawnPrefabOnEnemyDestroy : MonoBehaviour, IOnEnemyDestroy
    {
        [SerializeField] private GameObject _prefabToSpawn;
        
        public void OnEnemyDestroy()
        {
            Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}