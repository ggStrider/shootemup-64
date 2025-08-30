using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerShootEffectAnimator : SpriteAnimator
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            _signalBus.Subscribe<PlayerShootSignal>(SetupAndStartAnimation);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            _signalBus.TryUnsubscribe<PlayerShootSignal>(SetupAndStartAnimation);
        }
    }
}