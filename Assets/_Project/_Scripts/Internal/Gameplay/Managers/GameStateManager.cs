using System;
using Internal.Core.Scenes;
using Player;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        private SceneLoader _sceneLoader;
        
        private HealthSystem _playerHealth;
        private InputReader _inputReader;

        [Inject]
        private void Construct(HealthSystem playerHealth, 
            SceneLoader sceneLoader,
            InputReader inputReader)
        {
            _playerHealth = playerHealth;
            _sceneLoader = sceneLoader;
            _inputReader = inputReader;
        }

        private void OnEnable()
        {
            _playerHealth.OnDeath += LoseGame;
        }

        private void OnDisable()
        {
            if (_playerHealth)
            {
                _playerHealth.OnDeath -= LoseGame;
            }
        }

        private void LoseGame()
        {
            _inputReader.UnsubscribeInGameButtons();
            _sceneLoader.ReloadSceneWithTransition();
        }
    }
}