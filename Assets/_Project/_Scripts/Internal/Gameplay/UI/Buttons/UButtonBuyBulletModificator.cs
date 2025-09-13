using Definitions.BulletModificators.Scripts;
using Internal.Core.DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.Buttons
{
    public class UButtonBuyBulletModificator : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;

        [Space] 
        [SerializeField] private Image _itemImagePlaceholder;
        [SerializeField] private TextMeshProUGUI _namePlaceholder;
        [SerializeField] private TextMeshProUGUI _pricePlaceholder;
        
        [Space]
        [SerializeField] private BulletModificatorSO _toBuy;

        private PlayerData _playerData;

        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void Awake()
        {
            _itemImagePlaceholder.sprite = _toBuy.ItemIcon;
            _itemImagePlaceholder.color = _toBuy.IconColor;
            _namePlaceholder.text = _toBuy.ItemName;
            _pricePlaceholder.text = _toBuy.Price.ToString();
            
            _buttonToBind.onClick.AddListener(TryBuyModificator);
        }

        private void TryBuyModificator()
        {
            // seems strange to check it in this class. Maybe TODO: ?
            if (_playerData.Coins < _toBuy.Price)
            {
                Debug.Log($"[{nameof(UButtonBuyBulletModificator)}] Not enough coins!");
                return;
            }

            _playerData.AddBulletModificatorInInventory(_toBuy);
            _playerData.Coins.Value -= _toBuy.Price;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (_buttonToBind == null)
            {
                if (TryGetComponent<Button>(out var button)) _buttonToBind = button;
            }
        }
#endif
    }
}