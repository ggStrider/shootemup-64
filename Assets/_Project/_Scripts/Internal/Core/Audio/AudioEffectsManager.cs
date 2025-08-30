using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioEffectsManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _effectsSource;

        [Space] [SerializeField] private AudioPlayerAdvanced _onEnemyHitEffect;
        [SerializeField] private AudioPlayerAdvanced _onPlayerShootEffect;
        [SerializeField] private AudioPlayerAdvanced _onPlayerDamagedEffect;
        [SerializeField] private AudioPlayerAdvanced _onAnyEnemyDiesEffect;

        private SignalBus _signalBus;
        private PlayerHealth _playerHealth;

        [Inject]
        private void Construct(SignalBus signalBus, PlayerHealth playerHealth)
        {
            _signalBus = signalBus;
            _playerHealth = playerHealth;
        }

        private void OnEnable()
        {
            SubscribeOnSignalBusEvents();
            
            // TODO: Refactor Into Signal Bus
            _playerHealth.OnDamageTaken += PlayPlayerDamagedSound;
        }

        private void OnDisable()
        {
            UnsubscribeOnSignalBusEvents();
            _playerHealth.OnDamageTaken -= PlayPlayerDamagedSound;
        }

        private void SubscribeOnSignalBusEvents()
        {
            _signalBus.Subscribe<FakeEnemyHitInPlayerSignal>(PlayFakeEnemyHitInPlayerSound);
            _signalBus.Subscribe<PlayerShootSignal>(PlayPlayerShootSound);
            _signalBus.Subscribe<AnyEnemyDieSignal>(PlayAnyEnemyDieSound);
        }

        private void UnsubscribeOnSignalBusEvents()
        {
            _signalBus.TryUnsubscribe<FakeEnemyHitInPlayerSignal>(PlayFakeEnemyHitInPlayerSound);
            _signalBus.TryUnsubscribe<PlayerShootSignal>(PlayPlayerShootSound);
            _signalBus.TryUnsubscribe<AnyEnemyDieSignal>(PlayAnyEnemyDieSound);
        }

        private void PlayFakeEnemyHitInPlayerSound()
        {
            _onEnemyHitEffect.PlayShotOfRandomSound();
        }
        
        private void PlayPlayerShootSound()
        {
            _onPlayerShootEffect.PlayShotOfRandomSound();
        }
        
        private void PlayPlayerDamagedSound(int obj)
        {
            _onPlayerDamagedEffect.PlayShotOfRandomSound();
        }
        
        private void PlayAnyEnemyDieSound()
        {
            _onAnyEnemyDiesEffect.PlayShotOfRandomSound();
        }
    }
}