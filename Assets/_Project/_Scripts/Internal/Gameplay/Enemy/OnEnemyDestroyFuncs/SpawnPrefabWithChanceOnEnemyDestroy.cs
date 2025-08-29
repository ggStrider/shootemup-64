using System.Collections.Generic;
using Enemy;
using Internal.Core.Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace Internal.Gameplay.Enemy.OnEnemyDestroyFuncs
{
    public class SpawnPrefabWithChanceOnEnemyDestroy : MonoBehaviour, IOnEnemyDestroy
    {
        [SerializeField] private bool _randomPrefabFromList;

        [SerializeField, ShowIf(nameof(_randomPrefabFromList))]
        private List<GameObject> _prefabs = new();

        [SerializeField, HideIf(nameof(_randomPrefabFromList))]
        private GameObject _prefab;

        [Space] [Range(0f, 1f)] [SerializeField] private float _chance = 0.5f;
        
        public void OnEnemyDestroy()
        {
            if (Random.value > _chance) return;

            var prefab = _randomPrefabFromList ? _prefabs.GetRandomElement() : _prefab;
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}