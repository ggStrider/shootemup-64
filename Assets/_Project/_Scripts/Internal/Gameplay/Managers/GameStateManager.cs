using Internal.Core.Scenes;
using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using Player;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        private HealthSystem _playerHealth;
        private InputReader _inputReader;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(PlayerHealth playerHealth, 
            InputReader inputReader,
            SignalBus signalBus)
        {
            _playerHealth = playerHealth;
            _inputReader = inputReader;
            _signalBus = signalBus;
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

        public void WinGame()
        {
            _inputReader.UnsubscribeInGameButtons();
            _signalBus.Fire(new GameEndSignal(won: true));
        }

        private void LoseGame()
        {
            _inputReader.UnsubscribeInGameButtons();
            // _sceneLoader.ReloadSceneWithTransition();

            _signalBus.Fire(new GameEndSignal(won: false));
        }
    }
}
