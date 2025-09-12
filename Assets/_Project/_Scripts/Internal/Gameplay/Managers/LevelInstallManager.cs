using System.Collections;
using Audio;
using Internal.Core.Scenes;
using Internal.Core.Signals;
using Internal.Gameplay.LevelCreator;
using Spawners;
using UI.Images;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    // TODO: thats bad; refactor
    public class LevelInstallManager : MonoBehaviour
    {
        [SerializeField] private float _delayAfterTransitionToStart = .8f;

        private SceneCardHolder _sceneCardHolder;
        private MusicManager _musicManager;

        private EnemyWaveSpawner _enemyWaveSpawner;
        private BackgroundColorChanger _backgroundColorChanger;
        private CameraManager _cameraManager;
        private LevelWavesCreator _levelWavesCreator;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(MusicManager musicManager, SceneCardHolder sceneCardHolder, SignalBus signalBus)
        {
            _musicManager = musicManager;
            _sceneCardHolder = sceneCardHolder;

            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<GameEndSignal>(FadeCurrentClip);
            
            _enemyWaveSpawner = FindAnyObjectByType<EnemyWaveSpawner>();
            _backgroundColorChanger = FindAnyObjectByType<BackgroundColorChanger>();
            _cameraManager = FindAnyObjectByType<CameraManager>();
            _levelWavesCreator = FindAnyObjectByType<LevelWavesCreator>();
            
            FindAnyObjectByType<TransitionImageMover>().StartTransitionEnded += OnTransitionEnded;
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<GameEndSignal>(FadeCurrentClip);
        }

        private void FadeCurrentClip() => _musicManager.FadeCurrentClip();

        private void OnTransitionEnded()
        {
            _musicManager.
            StartCoroutine(WaitAndInit());
        }

        private IEnumerator WaitAndInit()
        {
            yield return new WaitForSeconds(_delayAfterTransitionToStart);

            _musicManager.StartAndUnFadeClip(_sceneCardHolder.CurrentSceneCard.LevelClip,
                _sceneCardHolder.CurrentSceneCard.Volume);
            
            _cameraManager.InitializeCameraBassShake();
            _enemyWaveSpawner?.InitializeSpawning();
            _backgroundColorChanger?.Initialize();
            _levelWavesCreator?.CompleteInitialization();
        }
    }
}