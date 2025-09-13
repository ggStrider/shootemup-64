using System;
using Audio;
using Definitions.Scenes.CameraBassShake;
using Definitions.Scenes.Delays.BackgroundChanger;
using Definitions.Waves;
using NaughtyAttributes;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
        [SerializeField] private DelaysWave _delaysWaveToWrite;
        [SerializeField] private BackgroundChangeDelays _bgDelays;
        
        private InputReader _inputReader;

        private enum StartCountingDelayMethods
        {
            OnEnable,
            WhenAnyButtonPressed,
            OnInitializationCompleted
        };

        // Runtime values
        private float _delayToNextEnemy = 0;
        private float _delayWaveToWrite = 0;
        private float _delayBg = 0;
        
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
            _delayWaveToWrite += Time.deltaTime;
            _delayBg += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _delaysWaveToWrite.Delays.Add(_delayWaveToWrite);
                _delayWaveToWrite = 0;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                var randomColor = Random.ColorHSV(
                    0f, 1f,
                    0.5f, 1f,
                    0.5f, 1f
                );
                randomColor.a = 1;
                
                _bgDelays.DelaysWith2T.Add(new(
                    _delayBg, randomColor));

                _delayBg = 0;
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