using DG.Tweening;
using Internal.Gameplay.EntitiesShared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Player
{
    public class UPlayerHealth : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthCountLabel;
        [SerializeField] private Image _healthIcon;
        
        [Space]
        [SerializeField] private float _animationTweenDuration = 0.2f;

        private PlayerHealth _playerHealth;

        private const string TEXT_BEFORE_HEALTH_POINTS = "x";

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
            _playerHealth.OnDamageTaken += PlayIconAnimation;

            UpdateHealthUI(_playerHealth.CurrentHealth);
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHealed -= UpdateHealthUI;
                _playerHealth.OnDamageTaken -= UpdateHealthUI;
                _playerHealth.OnDamageTaken -= PlayIconAnimation;
            }
        }

        private void UpdateHealthUI(int currentHealth)
        {
            _healthCountLabel.text = TEXT_BEFORE_HEALTH_POINTS + currentHealth;
        }
        
        private void PlayIconAnimation(int newHp)
        {
            var sequence = DOTween.Sequence();

            var startColor = _healthIcon.color;
            startColor.a = 1f;
            var fadedColor = _healthIcon.color;
            fadedColor.a = 36f / 255f;

            sequence.Append(_healthIcon.DOColor(fadedColor, _animationTweenDuration));
            sequence.Append(_healthIcon.DOColor(startColor, _animationTweenDuration));
            sequence.Append(_healthIcon.DOColor(fadedColor, _animationTweenDuration));
            sequence.Append(_healthIcon.DOColor(startColor, _animationTweenDuration));

            sequence.Play();
        }
    }
}