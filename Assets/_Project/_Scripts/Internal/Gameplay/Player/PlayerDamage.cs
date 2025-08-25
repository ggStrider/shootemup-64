using Audio;
using Internal.Gameplay;
using UI.Images;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerDamage : MonoBehaviour
    {
        [SerializeField] private AudioPlayerAdvanced _audioPlayer;
        private InputReader _inputReader;
        
        private HealthSystem _playerHealth;
        
        private const int DAMAGE_FROM_ENEMY = 1;

        [Inject]
        private void Construct(HealthSystem playerHealth, InputReader inputReader)
        {
            _playerHealth = playerHealth;
            _inputReader = inputReader;
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
            _inputReader.UnsubscribeInGameButtons();
            
            TransitionImageMover.Instance.OnOverlayed += ReloadScene;
            TransitionImageMover.Instance.MoveTo(TransitionImageMover.MoveToTypes.OverlayScreen);
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            if (TransitionImageMover.Instance != null)
            {
                TransitionImageMover.Instance.OnOverlayed -= ReloadScene;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Destroy object which entered the player
            if (other.CompareTag(StaticKeys.ENEMY_TAG) || other.CompareTag(StaticKeys.FAKE_ENEMY_TAG))
            {
                _audioPlayer?.PlayShotOfRandomSound();
                Tools.ToolsForEasyDebug.Destroy(other.gameObject);
            }

            if (other.CompareTag(StaticKeys.ENEMY_TAG))
            {
                _playerHealth?.Damage(DAMAGE_FROM_ENEMY);
                // Debug.Log($"[{GetType().Name}] Player damaged with {other.gameObject.name}; ");
            }
        }
    }
}