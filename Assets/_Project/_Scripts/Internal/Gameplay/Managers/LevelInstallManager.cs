using System.Collections;
using Audio;
using Internal.Core.Scenes;
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

        [Inject]
        private void Construct(MusicManager musicManager, SceneCardHolder sceneCardHolder)
        {
            _musicManager = musicManager;
            _sceneCardHolder = sceneCardHolder;
        }

        private void Awake()
        {
            FindAnyObjectByType<TransitionImageMover>().StartTransitionEnded += OnTransitionEnded;
        }

        private void OnTransitionEnded()
        {
            StartCoroutine(WaitAndInit());
        }

        private IEnumerator WaitAndInit()
        {
            yield return new WaitForSeconds(_delayAfterTransitionToStart);

            _musicManager.StartAndUnFadeClip(_sceneCardHolder.CurrentSceneCard.LevelClip,
                _sceneCardHolder.CurrentSceneCard.Volume);

            var enemyWaveSpawner = FindAnyObjectByType<EnemyWaveSpawner>();
            var backgroundColorChanger = FindAnyObjectByType<BackgroundColorChanger>();
            
            enemyWaveSpawner.InitializeSpawning();
            backgroundColorChanger.Initialize();
        }
    }
}