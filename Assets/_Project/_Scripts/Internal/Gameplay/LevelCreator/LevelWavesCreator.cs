using System;
using Audio;
using Definitions.Scenes.CameraBassShake;
using Definitions.Waves;
using NaughtyAttributes;
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
        [SerializeField, ShowIf(nameof(_useMusicManager))] private bool _stopMusicOnStart = true;
        [SerializeField] private bool _canPlayerShoot = true;
        
        [SerializeField] private EnemyWave _waveToWrite;
        [SerializeField] private CameraBassShakeWaves _cameraBassShakeWavesToWrite;
        
        private InputReader _inputReader;

        private enum StartCountingDelayMethods
        {
            OnEnable,
            WhenAnyButtonPressed,
            OnInitializationCompleted
        };

        // Runtime values
        private float _delayToNextEnemy = 0;
        private float _delayToNextBassCamera = 0;
        
        private bool _startedWriting;

        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public void CompleteInitialization()
        {
            if (_startCountingDelay == StartCountingDelayMethods.OnInitializationCompleted)
            {
                _startedWriting = true;
            }
        }

        private void Start()
        {
            var musicManager = FindAnyObjectByType<MusicManager>();
            if(_useMusicManager && _stopMusicOnStart) musicManager.StopPlayingMusic();
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
            if (_startCountingDelay == StartCountingDelayMethods.WhenAnyButtonPressed) _startedWriting = true;
            if (!_startedWriting) return;

            var sideToSpawn = GetSideToSpawn(direction);
            var enemyInWave = new EnemyInWave(sideToSpawn, _delayToNextEnemy);
            _waveToWrite.EnemiesInWave.Add(enemyInWave);

            _delayToNextEnemy = 0;
        }

        private void Update()
        {
            if (!_startedWriting) return;
            
            _delayToNextEnemy += Time.deltaTime;
            _delayToNextBassCamera += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cameraBassShakeWavesToWrite.Delays.Add(_delayToNextBassCamera);
                _delayToNextBassCamera = 0;
            }
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