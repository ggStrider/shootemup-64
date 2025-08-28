using Internal.Core.Signals;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioEffectsManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _effectsSource;

        [Space] [SerializeField] private AudioPlayerAdvanced _onEnemyHitEffect;

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
        }
        
        private void UnsubscribeOnSignalBusEvents()
        {
            _signalBus.TryUnsubscribe<EnemyHitInPlayerSignal>(PlayEnemyHitInPlayerSound);
        }

        private void PlayEnemyHitInPlayerSound()
        {
            _onEnemyHitEffect.PlayShotOfRandomSound();
        }
    }
}