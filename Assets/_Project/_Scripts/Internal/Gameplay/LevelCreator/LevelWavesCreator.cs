using Audio;
using Definitions.Waves;
using Player;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.LevelCreator
{
    public class LevelWavesCreator : MonoBehaviour
    {
        [SerializeField] private StartCountingDelayMethods _startCountingDelay = 
            StartCountingDelayMethods.WhenAnyButtonPressed;

        [Space] [SerializeField] private bool _useMusicManager = true;
        [SerializeField] private bool _canPlayerShoot = true;
        
        [SerializeField] private EnemyWave _waveToWrite;
        private InputReader _inputReader;

        private enum StartCountingDelayMethods
        {
            OnEnable,
            WhenAnyButtonPressed
        };

        // Runtime values
        private float _delayToNextEnemy = 0;
        private bool _startedWriting;

        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void OnEnable()
        {
            if (_startCountingDelay == StartCountingDelayMethods.OnEnable)
                _startedWriting = true;
            
            if(!_useMusicManager)
                FindAnyObjectByType<MusicManager>().gameObject.SetActive(false);
            
            if(!_canPlayerShoot)
                FindAnyObjectByType<PlayerShoot>().gameObject.SetActive(false);

            _inputReader.OnShootPressed += ReadIntoWaveSO;
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnShootPressed -= ReadIntoWaveSO;
            }
        }

        private void ReadIntoWaveSO(Vector2 direction)
        {
            // if(_startCountingDelay == StartCountingDelayMethods.WhenAnyButtonPressed)
            _startedWriting = true;

            var sideToSpawn = GetSideToSpawn(direction);
            var enemyInWave = new EnemyInWave(sideToSpawn, _delayToNextEnemy);
            _waveToWrite.EnemiesInWave.Add(enemyInWave);

            _delayToNextEnemy = 0;
        }

        private void Update()
        {
            if (!_startedWriting) return;
            _delayToNextEnemy += Time.deltaTime;
        }

        private SidesToSpawn GetSideToSpawn(Vector2 direction)
        {
            if (direction.x > 0) return SidesToSpawn.Right;
            if (direction.x < 0) return SidesToSpawn.Left;
            if (direction.y > 0) return SidesToSpawn.Up;
            return SidesToSpawn.Down;
        }
    }
}