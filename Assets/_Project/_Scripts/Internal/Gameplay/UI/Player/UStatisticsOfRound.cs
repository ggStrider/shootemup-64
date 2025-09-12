using Internal.Core.Signals;
using Internal.Gameplay.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class UStatisticsOfRound : MonoBehaviour
    {
        [SerializeField] private GameObject _parentOfStatistic;
        
        [Space]
        [SerializeField] private TextMeshProUGUI _killedRealEnemiesLabel;
        [SerializeField] private TextMeshProUGUI _killedFakeEnemiesLabel;

        [Space] 
        [SerializeField] private TextMeshProUGUI _realEnemiesHitInPlayerLabel;
        [SerializeField] private TextMeshProUGUI _fakeEnemiesHitInPlayerLabel;
        
        [Space]
        [SerializeField] private TextMeshProUGUI _bulletsShotLabel;
        [SerializeField] private TextMeshProUGUI _backgroundChangedLabel;

        [Space] [SerializeField] private TextMeshProUGUI _earnedMoneyLabel;

        private PlayerStatisticObserver _statisticObserver;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(PlayerStatisticObserver statisticObserver, SignalBus signalBus)
        {
            _statisticObserver = statisticObserver;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<GameEndSignal>(SetStatisticToLabels);
        }

        private void OnDisable()
        {
            _signalBus?.TryUnsubscribe<GameEndSignal>(SetStatisticToLabels);
        }

        private void SetStatisticToLabels()
        {
            _parentOfStatistic.SetActive(true);
            
                _killedRealEnemiesLabel.text = $"Killed real enemies: {_statisticObserver.RealEnemiesKilled}";
            _killedFakeEnemiesLabel.text = $"Killed fake enemies: {_statisticObserver.FakeEnemiesKilled}";

            _realEnemiesHitInPlayerLabel.text = $"Real enemies hit player: {_statisticObserver.RealEnemiesHitInPlayer}";
            _fakeEnemiesHitInPlayerLabel.text = $"Fake enemies hit player: {_statisticObserver.FakeEnemiesHitInPlayer}";

            _bulletsShotLabel.text = $"Bullets shot: {_statisticObserver.PlayerShot}";
            _backgroundChangedLabel.text = $"Backgrounds changed: {_statisticObserver.BackgroundChanged}";

            _earnedMoneyLabel.text = $"Earned coins: {_statisticObserver.EarnedCoins}";
        }
    }
}