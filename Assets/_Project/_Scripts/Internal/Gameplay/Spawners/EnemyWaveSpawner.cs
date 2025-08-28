using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Waves;
using Enemy;
using Internal.Core.Pools;
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

        // Pools
        private SimpleEnemyPool _simpleEnemyPool;
        private FakeEnemyPool _fakeEnemyPool;
        
        private CancellationTokenSource _changingBackgroundCts;
        private SignalBus _signalBus;

        private const int FIRST_WAVE_INDEX = 0;

        [Inject]
        private void Construct(SimpleEnemyPool simpleEnemyPool, FakeEnemyPool fakeEnemyPool, SignalBus signalBus)
        {
            _simpleEnemyPool = simpleEnemyPool;
            _fakeEnemyPool = fakeEnemyPool;
            _signalBus = signalBus;
        }

        // TODO: Refactor into some smth else, not coroutine, idk rn
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _changingBackgroundCts,
                createNewTokenAfter: true);

            SpawnWave(FIRST_WAVE_INDEX).Forget();
            RandomizeBackgroundColorCoroutine(FIRST_WAVE_INDEX, _changingBackgroundCts.Token).Forget();
            // InvokeRepeating(nameof(RandomizeBackgroundColor), _changeBackgroundColorInterval, _changeBackgroundColorInterval);
        }

        private async UniTask RandomizeBackgroundColorCoroutine(int waveIndex, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    _backgroundRenderer.color = GetRandomColor();
                    _signalBus.Fire(new BackgroundChangedSignal(
                        _backgroundRenderer, _backgroundRenderer.color));
                    
                    await UniTask.WaitForSeconds(
                        _levelWaves[waveIndex].DelayToChangeBackground, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask SpawnWave(int waveIndex)
        {
            for (var enemyIndex = 0; enemyIndex < _levelWaves[waveIndex].EnemiesInWave.Count; enemyIndex++)
            {
                var enemy = _levelWaves[waveIndex].EnemiesInWave[enemyIndex];
                
                CreateAndInitializeEnemy(enemy, waveIndex, enemyIndex);
                await UniTask.WaitForSeconds(enemy.DelayToNextEnemy);
            }

            WaitForNextWave(waveIndex).Forget();
        }

        private async UniTask WaitForNextWave(int currentWaveIndex)
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _changingBackgroundCts,
                createNewTokenAfter: true);

            await UniTask.WaitForSeconds(_levelWaves[currentWaveIndex].DelayToNextWave);

            if (currentWaveIndex + 1 < _levelWaves.Length)
            {
                SpawnWave(currentWaveIndex + 1).Forget();
                RandomizeBackgroundColorCoroutine(currentWaveIndex + 1,
                    _changingBackgroundCts.Token).Forget();
            }
            else
            {
                Debug.Log("All waves completed!");
            }
        }

        private void CreateAndInitializeEnemy(EnemyInWave enemy, int waveIndex, int enemyIndex)
        {
            bool shouldSpawnRealEnemy;
            if (_levelWaves.UseRandomEnemyType) shouldSpawnRealEnemy = Random.Range(0, 3) == 1;
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
                unit.ChangeSkinColor(GetRandomColor());
            }
            
            unit.transform.SetPositionAndRotation(randomPoint.position, Quaternion.identity);
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

        private Color GetRandomColor()
        {
            var r = Random.Range(0f, 256f);
            var g = Random.Range(0f, 256f);
            var b = Random.Range(0f, 256f);

            return new Color(r / 255, g / 255, b / 255);
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