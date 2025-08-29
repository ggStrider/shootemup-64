using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Definitions.Waves
{
    [CreateAssetMenu(fileName = "New Level Waves", menuName = StaticKeys.PROJECT_NAME + 
                                                             "/Definitions/Spawners/Level Enemy Waves")]
    public class LevelWaves : ScriptableObject, IEnumerable<EnemyWave>
    {
        [SerializeField] private EnemyWave[] _waves;
        
        [field: SerializeField] public bool UseRandomEnemyType = true;

        [field: ShowIf(nameof(UseRandomEnemyType)), SerializeField, Range(0f, 1f)]
        public float RealEnemyChance = 0.6f;

        public static implicit operator EnemyWave[](LevelWaves level)
        {
            return level._waves;
        }

        public EnemyWave this[int index] => _waves[index];
        public int Length => _waves.Length;

        public IEnumerator<EnemyWave> GetEnumerator()
        {
            foreach (var wave in _waves)
                yield return wave;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}