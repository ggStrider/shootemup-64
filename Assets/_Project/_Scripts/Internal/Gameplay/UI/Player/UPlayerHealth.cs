using Internal.Gameplay.EntitiesShared;
using Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Player
{
    public class UPlayerHealth : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthCountLabel;
        private PlayerHealth _playerHealth;
        
        [Inject]
        private void Construct(PlayerHealth playerHealth)
        {
            _playerHealth = playerHealth;
        }

        // TODO: I guess I need some bootstrap here. Need Research here 
        private void Start()
        {
            _playerHealth.OnHealed += UpdateHealthUI;
            _playerHealth.OnDamageTaken += UpdateHealthUI;
            
            UpdateHealthUI(_playerHealth.CurrentHealth);
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHealed -= UpdateHealthUI;
                _playerHealth.OnDamageTaken -= UpdateHealthUI;
            }
        }

        private void UpdateHealthUI(int currentHealth)
        {
            _healthCountLabel.text = $"Health: {currentHealth}";
        }
    }
}