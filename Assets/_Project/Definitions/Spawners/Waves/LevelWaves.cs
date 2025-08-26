using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Waves
{
    [CreateAssetMenu(fileName = "New Level Waves", menuName = StaticKeys.PROJECT_NAME + 
                                                             "/Definitions/Spawners/Level Enemy Waves")]
    public class LevelWaves : ScriptableObject, IEnumerable<EnemyWave>
    {
        [SerializeField] private EnemyWave[] _waves;

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