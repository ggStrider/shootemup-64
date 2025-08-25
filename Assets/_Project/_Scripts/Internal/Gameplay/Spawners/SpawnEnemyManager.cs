using Enemy;
using Player;
using UnityEngine;

namespace Spawners
{
    public class SpawnEnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyAI _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        [Space] [SerializeField] private uint _enemiesPerWave = 5;
        
        [SerializeField] private float _spawnInterval = 0.7f;
        [SerializeField] private float _changeBackgroundColorInterval = 5f;

        [Space] [SerializeField] private Transform _player;
        [Space] [SerializeField] private SpriteRenderer _backgroundRenderer;

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), _spawnInterval, _spawnInterval);
            InvokeRepeating(nameof(ChangeBackgroundColor), _changeBackgroundColorInterval, _changeBackgroundColorInterval);
        }

        private void ChangeBackgroundColor()
        {
            _backgroundRenderer.color = GetRandomColor();
        }

        private void Spawn()
        {
            for (var i = 1; i <= _enemiesPerWave; i++)
            {
                var spawnEnemy = Random.Range(0, 3) == 1;
                var randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
                var unit = Instantiate(_enemyPrefab, randomPoint, Quaternion.identity);
            
                unit.Initialize(_player);

                var unitSpriteRenderer = unit.GetComponent<SpriteRenderer>();
                if (!spawnEnemy)
                {
                    unit.tag = StaticKeys.FAKE_ENEMY_TAG;
                    unitSpriteRenderer.color = GetRandomColor();
                }
                else
                {
                    unitSpriteRenderer.color = _backgroundRenderer.color;
                }   
            }
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
            _player ??= FindAnyObjectByType<PlayerShoot>().transform;
        }
#endif
    }
}
