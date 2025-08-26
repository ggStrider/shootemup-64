using Cysharp.Threading.Tasks;
using Definitions.Waves;
using Internal.Core.Pools;
using Player;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class SpawnEnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform _up;
        [SerializeField] private Transform _down;
        [SerializeField] private Transform _right;
        [SerializeField] private Transform _left;

        [Space(10)]
        [SerializeField] private LevelWaves _levelWaves;
        [SerializeField] private float _changeBackgroundColorInterval = 5f;

        [Space] [SerializeField] private Transform _player;
        [Space] [SerializeField] private SpriteRenderer _backgroundRenderer;

        private EnemyPool _enemyPool;

        [Inject]
        private void Construct(EnemyPool enemyPool)
        {
            _enemyPool = enemyPool;
        }

        private void Start()
        {
            SpawnWave().Forget();
            InvokeRepeating(nameof(ChangeBackgroundColor), _changeBackgroundColorInterval, _changeBackgroundColorInterval);
        }

        private void ChangeBackgroundColor()
        {
            _backgroundRenderer.color = GetRandomColor();
        }

        private async UniTask SpawnWave()
        {
            foreach (var wave in _levelWaves)
            {
                foreach (var enemy in wave.EnemiesInWave)
                {
                    CreateAndInitializeEnemy(enemy);
                    await UniTask.WaitForSeconds(enemy.DelayToNextEnemy);
                }
            }
        }

        private void CreateAndInitializeEnemy(EnemyInWave enemy)
        {
            var shouldSpawnEnemy = Random.Range(0, 3) == 1;
            var randomPoint = GetTransformPointByEnum(enemy.SideToSpawn);

            var unit = _enemyPool.Spawn();
            unit.transform.SetPositionAndRotation(randomPoint.position, Quaternion.identity);
            
            unit.Initialize(_player);

            var unitSpriteRenderer = unit.GetComponent<SpriteRenderer>();
            if (!shouldSpawnEnemy)
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
