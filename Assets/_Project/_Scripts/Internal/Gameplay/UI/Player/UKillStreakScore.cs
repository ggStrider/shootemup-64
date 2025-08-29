using DG.Tweening;
using Internal.Core.Signals;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.UI.Player
{
    public class UKillStreakScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killPlaceholder;

        [Space] 
        [Foldout("Punch settings")] [SerializeField] private float _punchDuration = 0.1f;
        [Foldout("Punch settings")] [SerializeField] private Vector3 _startScale = Vector3.one;
        [Foldout("Punch settings")] [SerializeField] private Vector3 _endScale = new(1.35f, 1.35f, 1);
        [Foldout("Punch settings")] [SerializeField] private int _vibrato = 1;
        [Foldout("Punch settings")] [SerializeField, Range(0f, 1f)] private float _elasticity = 0.5f;
        
        private int _streakCount = 0;

        private SignalBus _signalBus;

        private const string BEFORE_STREAK_TEXT = "x";

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<FakeEnemyDieSignal>(OnFakeEnemyDies);
            _signalBus.Subscribe<EnemyDieSignal>(OnRealEnemyDies);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<FakeEnemyDieSignal>(OnFakeEnemyDies);
            _signalBus.TryUnsubscribe<EnemyDieSignal>(OnRealEnemyDies);
        }

        private void OnFakeEnemyDies()
        {
            _streakCount = 0;
            UpdateStreakText();
        }
        
        private void OnRealEnemyDies()
        {
            _streakCount++;
            UpdateStreakText();
        }

        private void UpdateStreakText()
        {
            _killPlaceholder.text = BEFORE_STREAK_TEXT + _streakCount;
            _killPlaceholder.rectTransform.DOPunchScale(
                _endScale - _startScale,
                _punchDuration,
                _vibrato,
                _elasticity
            );
        }
    }
}