using UnityEngine;
using System;
using System.Collections.Generic;

namespace Definitions.Waves
{
    [CreateAssetMenu(fileName = "New Enemy Wave", menuName = StaticKeys.PROJECT_NAME + 
                                                             "/Definitions/Spawners/Enemy Wave")]
    public class EnemyWave : ScriptableObject
    {
        [field: SerializeField] public List<EnemyInWave> EnemiesInWave { get; private set; }
        [field: SerializeField] public float DelayToNextWave { get; private set; } = 3f;
        [field: SerializeField] public float DelayToChangeBackground { get; private set; } = 5f;
    }

    [Serializable]
    public class EnemyInWave
    {
        [field: SerializeField] public bool IsFakeEnemy { get; private set; } = true;
        [field: SerializeField] public SidesToSpawn SideToSpawn { get; private set; }
        [field: SerializeField] public float DelayToNextEnemy { get; private set; } = 0.2f;

        public EnemyInWave(bool isFakeEnemy, SidesToSpawn sideToSpawn, float delayToNextEnemy)
        {
            IsFakeEnemy = isFakeEnemy;
            SideToSpawn = sideToSpawn;
            DelayToNextEnemy = delayToNextEnemy;
        }

        public EnemyInWave(SidesToSpawn sideToSpawn, float delayToNextEnemy)
        {
            SideToSpawn = sideToSpawn;
            DelayToNextEnemy = delayToNextEnemy;
        }
    }

    public enum SidesToSpawn
    {
        Up,
        Down,
        Right,
        Left
    };
}