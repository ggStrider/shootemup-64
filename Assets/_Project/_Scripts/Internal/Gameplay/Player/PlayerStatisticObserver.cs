using Internal.Core.DataModel;
using Internal.Core.Signals;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Player
{
    public class PlayerStatisticObserver : MonoBehaviour
    {
        [field: SerializeField] public int RealEnemiesKilled { get; private set; }
        [field: SerializeField] public int FakeEnemiesKilled { get; private set; }

        [field: Space] 
        [field: SerializeField] public int RealEnemiesHitInPlayer { get; private set; }
        [field: SerializeField] public int FakeEnemiesHitInPlayer { get; private set; }
        
        [field: Space]
        [field: SerializeField] public int PlayerShot { get; private set; }
        [field: SerializeField] public int BackgroundChanged { get; private set; }
        
        public int EarnedCoins => _playerData.Coins - _startCoins;

        private PlayerData _playerData;
        private SignalBus _signalBus;

        private int _startCoins;
        
        [Inject]
        private void Construct(SignalBus signalBus, PlayerData playerData)
        {
            _signalBus = signalBus;
            _playerData = playerData;
        }

        private void Awake()
        {
            _startCoins = _playerData.Coins;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<RealEnemyKilledSignal>(AddRealEnemyKilledCount);
            _signalBus.Subscribe<FakeEnemyKilledSignal>(AddFakeEnemyKilledCount);
            
            _signalBus.Subscribe<RealEnemyHitInPlayerSignal>(AddRealEnemyHitInPlayerCount);
            _signalBus.Subscribe<FakeEnemyHitInPlayerSignal>(AddFakeEnemyHitInPlayerCount);
            
            _signalBus.Subscribe<PlayerShootSignal>(AddPlayerShotCount);
            _signalBus.Subscribe<BackgroundChangedSignal>(AddBackgroundChangedCount);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<RealEnemyKilledSignal>(AddRealEnemyKilledCount);
            _signalBus.TryUnsubscribe<FakeEnemyKilledSignal>(AddFakeEnemyKilledCount);
            
            _signalBus.TryUnsubscribe<RealEnemyHitInPlayerSignal>(AddRealEnemyHitInPlayerCount);
            _signalBus.TryUnsubscribe<FakeEnemyHitInPlayerSignal>(AddFakeEnemyHitInPlayerCount);
            
            _signalBus.TryUnsubscribe<PlayerShootSignal>(AddPlayerShotCount);
            _signalBus.TryUnsubscribe<BackgroundChangedSignal>(AddBackgroundChangedCount);
        }

        private void AddRealEnemyKilledCount() => RealEnemiesKilled++;
        private void AddFakeEnemyKilledCount() => FakeEnemiesKilled++;
        
        private void AddRealEnemyHitInPlayerCount() => RealEnemiesHitInPlayer++;
        private void AddFakeEnemyHitInPlayerCount() => FakeEnemiesHitInPlayer++;

        private void AddPlayerShotCount() => PlayerShot++;

        private void AddBackgroundChangedCount(BackgroundChangedSignal signal)
        {
            if (signal.CompletelyChanged)
            {
                BackgroundChanged++;
            }
        }
    }
}