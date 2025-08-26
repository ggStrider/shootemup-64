using Audio;
using Enemy;
using Internal.Core.Scenes;
using Internal.Gameplay;
using UI.Images;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Player
{
    // TODO: This class is ass tbh
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerDamage : MonoBehaviour
    {
        [SerializeField] private AudioPlayerAdvanced _audioPlayer;
        private InputReader _inputReader;
        
        private HealthSystem _playerHealth;
        
        private const int DAMAGE_FROM_ENEMY = 1;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(HealthSystem playerHealth, InputReader inputReader, SceneLoader sceneLoader)
        {
            _playerHealth = playerHealth;
            _inputReader = inputReader;
            _sceneLoader = sceneLoader;
        }

        private void OnEnable()
        {
            _playerHealth.OnDeath += Die;
        }
        
        private void OnDisable()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnDeath -= Die;
            }
        }
        
        private void Die()
        {
            _sceneLoader.ReloadSceneWithTransition();
            _inputReader.UnsubscribeInGameButtons();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyAI>(out var enemy))
            {
                _audioPlayer?.PlayShotOfRandomSound();

                if (other.CompareTag(StaticKeys.ENEMY_TAG))
                {
                    _playerHealth?.Damage(DAMAGE_FROM_ENEMY);
                    // Debug.Log($"[{GetType().Name}] Player damaged with {other.gameObject.name}; ");
                }
                
                enemy.DespawnSelf();
            }
        }
    }
}