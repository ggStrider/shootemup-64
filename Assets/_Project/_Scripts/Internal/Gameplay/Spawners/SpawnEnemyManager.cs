using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Waves;
using Internal.Core.Pools;
using Player;
using Tools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class SpawnEnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform _up;
        [SerializeField] private Transform _down;
        [SerializeField] private Transform _right;
        [SerializeField] private Transform _left;

        [Space(10)] [SerializeField] private LevelWaves _levelWaves;

        [Space] [SerializeField] private Transform _player;
        [Space] [SerializeField] private SpriteRenderer _backgroundRenderer;

        private EnemyPool _enemyPool;
        private CancellationTokenSource _changingBackgroundCts;

        private const int FIRST_WAVE_INDEX = 0;

        [Inject]
        private void Construct(EnemyPool enemyPool)
        {
            _enemyPool = enemyPool;
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
            foreach (var enemy in _levelWaves[waveIndex].EnemiesInWave)
            {
                CreateAndInitializeEnemy(enemy);
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

        private void CreateAndInitializeEnemy(EnemyInWave enemy)
        {
            var shouldSpawnRealEnemy = Random.Range(0, 3) == 1;
            var randomPoint = GetTransformPointByEnum(enemy.SideToSpawn);

            var unit = _enemyPool.Spawn();
            unit.transform.SetPositionAndRotation(randomPoint.position, Quaternion.identity);

            unit.Initialize(_player);

            var unitSpriteRenderer = unit.GetComponent<SpriteRenderer>();
            if (!shouldSpawnRealEnemy)
            {
                unit.tag = StaticKeys.FAKE_ENEMY_TAG;
                unitSpriteRenderer.color = GetRandomColor();
            }
            else
            {
                unit.tag = StaticKeys.ENEMY_TAG;
                unitSpriteRenderer.color = _backgroundRenderer.color;
            }
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