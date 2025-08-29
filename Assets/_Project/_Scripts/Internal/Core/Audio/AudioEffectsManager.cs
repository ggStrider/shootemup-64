using Internal.Core.Signals;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioEffectsManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _effectsSource;

        [Space] [SerializeField] private AudioPlayerAdvanced _onEnemyHitEffect;
        [SerializeField] private AudioPlayerAdvanced _onPlayerShootEffect;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeOnSignalBusEvents();
        }

        private void OnDisable()
        {
            UnsubscribeOnSignalBusEvents();
        }

        private void SubscribeOnSignalBusEvents()
        {
            _signalBus.Subscribe<EnemyHitInPlayerSignal>(PlayEnemyHitInPlayerSound);
            _signalBus.Subscribe<PlayerShootSignal>(PlayPlayerShootSound);
        }

        private void UnsubscribeOnSignalBusEvents()
        {
            _signalBus.TryUnsubscribe<EnemyHitInPlayerSignal>(PlayEnemyHitInPlayerSound);
            _signalBus.TryUnsubscribe<PlayerShootSignal>(PlayPlayerShootSound);
        }

        private void PlayEnemyHitInPlayerSound()
        {
            _onEnemyHitEffect.PlayShotOfRandomSound();
        }
        
        private void PlayPlayerShootSound()
        {
            _onPlayerShootEffect.PlayShotOfRandomSound();
        }
    }
}