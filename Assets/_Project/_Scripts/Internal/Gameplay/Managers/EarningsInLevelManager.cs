using Definitions.Scenes.Cards;
using Internal.Core.DataModel;
using Internal.Core.Scenes;
using Internal.Core.Signals;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class EarningsInLevelManager : MonoBehaviour
    {
        [SerializeField] private SceneCard _sceneCard;

        [Space] [ReadOnly] 
        [SerializeField] private PlayerData _playerData;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus, SceneCardHolder sceneCardHolder, PlayerData playerData)
        {
            _signalBus = signalBus;
            _playerData = playerData;

            if (sceneCardHolder.CurrentSceneCard == null)
            {
                Debug.LogError($"[{GetType().Name}] No SceneCard in Injected {nameof(SceneCardHolder)}! " +
                               $"Probably you starting scene not from menu...");
            }
            else
            {
                _sceneCard = sceneCardHolder.CurrentSceneCard;
            }
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<RealEnemyKilledSignal>(OnRealEnemyKilled);
            _signalBus.Subscribe<RealEnemyHitInPlayerSignal>(SubtractRealEnemyHitInPlayer);
            
            _signalBus.Subscribe<FakeEnemyKilledSignal>(OnFakeEnemyKilledSignal);
            _signalBus.Subscribe<FakeEnemyHitInPlayerSignal>(OnFakeEnemyHitInPlayer);
        }
        
        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<RealEnemyKilledSignal>(OnRealEnemyKilled);
            _signalBus.TryUnsubscribe<RealEnemyHitInPlayerSignal>(SubtractRealEnemyHitInPlayer);
            
            _signalBus.TryUnsubscribe<FakeEnemyKilledSignal>(OnFakeEnemyKilledSignal);
            _signalBus.TryUnsubscribe<FakeEnemyHitInPlayerSignal>(OnFakeEnemyHitInPlayer);
        }

        #region Real Enemy
        
        private void SubtractRealEnemyHitInPlayer()
        {
            _playerData.SubtractCoins(_sceneCard.EarningSettingsInSceneCard
                .SubtractCoinsOnRealEnemyHitInPlayer);
        }

        private void OnRealEnemyKilled()
        {
            _playerData.AddCoins(_sceneCard.EarningSettingsInSceneCard
                .AddCoinsOnRealEnemyKilled);
        }
        
        #endregion
        
        #region Fake Enemy
        
        private void OnFakeEnemyHitInPlayer()
        {
            _playerData.AddCoins(_sceneCard.EarningSettingsInSceneCard
                .AddCoinsOnFakeEnemyHitInPlayer);
        }

        private void OnFakeEnemyKilledSignal()
        {
            _playerData.SubtractCoins(_sceneCard.EarningSettingsInSceneCard
                .SubtractCoinsOnFakeEnemyKilled);
        }

        #endregion
    }
}