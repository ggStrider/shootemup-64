using Internal.Core.Extensions;
using Internal.Core.Pools;
using Internal.Core.Signals;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class FakeEnemy : EnemyBase
    {
        private FakeEnemyPool _fakeEnemyPool;
        
        [Inject]
        private void Construct(FakeEnemyPool fakeEnemyPool)
        {
            _fakeEnemyPool = fakeEnemyPool;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SignalBus.Subscribe<BackgroundChangedSignal>(SetAnalogueColor);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SignalBus.TryUnsubscribe<BackgroundChangedSignal>(SetAnalogueColor);
        }

        public void SetAnalogueColor(Color newColor)
        {
            ChangeSkinColor(newColor.GetComplementary());
        }
        
        private void SetAnalogueColor(BackgroundChangedSignal bg)
        {
            ChangeSkinColor(bg.NewColor.GetComplementary());
        }

        protected override void OnDie()
        {
            SignalBus.Fire(new FakeEnemyDieSignal(this));
            InvokeAllIOnDestroy();
            DespawnSelf();
        }

        protected override void DespawnSelf()
        {
            if (!gameObject.activeInHierarchy) return;
            _fakeEnemyPool.Despawn(this);
        }

        protected override void OnHitInPlayer(GameObject player)
        {
            base.OnHitInPlayer(player);
            SignalBus.Fire(new FakeEnemyHitInPlayerSignal(this));
        }
    }
}