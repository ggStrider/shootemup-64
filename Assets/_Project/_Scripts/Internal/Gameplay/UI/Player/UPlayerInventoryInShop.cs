using Definitions.BulletModificators.Scripts;
using Internal.Core.DataModel;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.UI.Player
{
    public class UPlayerInventoryInShop : MonoBehaviour
    {
        [SerializeField] private UInventoryItem _coins;
        [SerializeField] private UInventoryItem _pierce;
        [SerializeField] private UInventoryItem _slowDownPuddle;
        [SerializeField] private UInventoryItem _speedUpPuddle;
        
        private PlayerData _playerData;
        
        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void OnEnable()
        {
            _playerData.Coins.OnValueChanged += UpdateAllAmounts;
            _playerData.BulletModificators.OnItemAdded += OnBulletModificatorAdded;
            _playerData.BulletModificators.OnItemRemoved += OnBulletModificatorRemoved;
            
            UpdateAllAmounts(0, 0);
            UpdateBulletModificatorsAmounts();
            SetAllSprites();
        }
        
        private void OnBulletModificatorAdded(int arg1, BulletModificatorInInventory arg2) =>
            UpdateBulletModificatorsAmounts();

        private void OnBulletModificatorRemoved(BulletModificatorInInventory obj) =>
            UpdateBulletModificatorsAmounts();

        private void OnDisable()
        {
            if (_playerData != null)
            {
                _playerData.Coins.OnValueChanged -= UpdateAllAmounts;
                _playerData.BulletModificators.OnItemAdded -= OnBulletModificatorAdded;
                _playerData.BulletModificators.OnItemRemoved -= OnBulletModificatorRemoved;
            }
        }
        
        private void UpdateAllAmounts(int oldValue, int newValue)
        {
            _coins.SetAmount(_playerData.Coins);
            UpdateBulletModificatorsAmounts();
        }

        private void UpdateBulletModificatorsAmounts()
        {
            var pierce = _playerData.TryGetBulletModificatorInInventoryBy(
                (BulletModificatorSO)_pierce.GameItem);

            var slowDownPuddle = _playerData.TryGetBulletModificatorInInventoryBy(
                (BulletModificatorSO)_slowDownPuddle.GameItem);
            
            var speedUpPuddle = _playerData.TryGetBulletModificatorInInventoryBy(
                (BulletModificatorSO)_speedUpPuddle.GameItem);

            _pierce.SetAmount(pierce?.Amount ?? 0);
            _slowDownPuddle.SetAmount(slowDownPuddle?.Amount ?? 0);
            _speedUpPuddle.SetAmount(speedUpPuddle?.Amount ?? 0);
        }

        private void SetAllSprites()
        {
            _coins.SetSpriteFromGameItem();
            _pierce.SetSpriteFromGameItem();
            _slowDownPuddle.SetSpriteFromGameItem();
            _speedUpPuddle.SetSpriteFromGameItem();
        }
    }
}