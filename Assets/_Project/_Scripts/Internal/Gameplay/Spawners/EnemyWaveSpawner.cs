using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Waves;
using Enemy;
using Internal.Core.Pools;
using Internal.Core.Scenes;
using Internal.Core.Signals;
using Player;
using Tools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class EnemyWaveSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _up;
        [SerializeField] private Transform _down;
        [SerializeField] private Transform _right;
        [SerializeField] private Transform _left;

        [Space(10)] [SerializeField] private LevelWaves _levelWaves;

        [Space] [SerializeField] private Transform _player;
        [Space] [SerializeField] private SpriteRenderer _backgroundRenderer;

        private SignalBus _signalBus;

        // Pools
        private SimpleEnemyPool _simpleEnemyPool;
        private FakeEnemyPool _fakeEnemyPool;

        private CancellationTokenSource _spawnCts;

        [Inject]
        private void Construct(SimpleEnemyPool simpleEnemyPool, FakeEnemyPool fakeEnemyPool,
            SceneCardHolder sceneCardHolder, SignalBus signalBus)
        {
            _simpleEnemyPool = simpleEnemyPool;
            _fakeEnemyPool = fakeEnemyPool;
            _levelWaves = sceneCardHolder.CurrentSceneCard.LevelWaves;

            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<GameEndSignal>(StopSpawning);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<GameEndSignal>(StopSpawning);
            StopSpawning();
        }

        private void StopSpawning()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _spawnCts);
        }

        public void InitializeSpawning()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _spawnCts, createNewTokenAfter: true);
            SpawnWave(0, _spawnCts.Token).Forget();
        }

        private async UniTask SpawnWave(int waveIndex, CancellationToken token)
        {
            try
            {
                for (var enemyIndex = 0; enemyIndex < _levelWaves[waveIndex].EnemiesInWave.Count; enemyIndex++)
                {
                    var enemy = _levelWaves[waveIndex].EnemiesInWave[enemyIndex];
                
                    CreateAndInitializeEnemy(enemy, waveIndex, enemyIndex);
                    await UniTask.WaitForSeconds(enemy.DelayToNextEnemy, cancellationToken: token);
                }

                WaitForNextWave(waveIndex, token).Forget();
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask WaitForNextWave(int currentWaveIndex, CancellationToken token)
        {
            await UniTask.WaitForSeconds(_levelWaves[currentWaveIndex].DelayToNextWave, cancellationToken: token);

            if (currentWaveIndex + 1 < _levelWaves.Length)
            {
                SpawnWave(currentWaveIndex + 1, token).Forget();
            }
            else
            {
                Debug.Log("All waves completed!");
            }
        }

        private void CreateAndInitializeEnemy(EnemyInWave enemy, int waveIndex, int enemyIndex)
        {
            bool shouldSpawnRealEnemy;
            if (_levelWaves.UseRandomEnemyType && Random.value <= _levelWaves.RealEnemyChance) 
                shouldSpawnRealEnemy = true;

            else shouldSpawnRealEnemy = !_levelWaves[waveIndex].EnemiesInWave[enemyIndex].IsFakeEnemy;

            var randomPoint = GetTransformPointByEnum(enemy.SideToSpawn);
            EnemyBase unit;
            
            if (shouldSpawnRealEnemy)
            {
                unit = _simpleEnemyPool.Spawn();
                (unit as SimpleMovingEnemy)?.ChangeSkinColor(_backgroundRenderer.color);
            }
            else
            {
                unit = _fakeEnemyPool.Spawn();
                (unit as FakeEnemy)?.SetAnalogueColor(_backgroundRenderer.color);
            }
            
            unit!.transform.SetPositionAndRotation(randomPoint.position, Quaternion.identity);
            unit.Initialize(_player);
        }

        private Transform GetTransformPointByEnum(SidesToSpawn sideToSpawn)
        {
            return sideToSpawn switch
            {
                SidesToSpawn.Up => _up,
                SidesToSpawn.Down => _down,
                SidesToSpawn.Left => _left,
                SidesToSpawn.Right => _right,
                _ => _down
            };
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (!_player)
            {
                _player = FindAnyObjectByType<PlayerShoot>().transform;
            }
        }
#endif
    }
}