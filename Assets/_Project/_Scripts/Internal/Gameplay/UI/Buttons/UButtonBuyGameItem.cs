using Definitions;
using Internal.Core.DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.Buttons
{
    public class UButtonBuyGameItem : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;
        
        [Space]
        [SerializeField] private Image _itemImagePlaceholder;
        [SerializeField] private TextMeshProUGUI _namePlaceholder;
        [SerializeField] private TextMeshProUGUI _pricePlaceholder;
        
        [Space]
        [SerializeField] private BuyableGameItem _toBuy;

        private PlayerData _playerData;

        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void SetToBuyAndInitialize(BuyableGameItem buyableGameItem)
        {
            _toBuy = buyableGameItem;
            Initialize();
        }

        private void Initialize()
        {
            _itemImagePlaceholder.sprite = _toBuy.ItemIcon;
            _itemImagePlaceholder.color = _toBuy.IconColor;
            _namePlaceholder.text = _toBuy.ItemName;
            _pricePlaceholder.text = _toBuy.Price.ToString();
            
            _buttonToBind.onClick.AddListener(TryBuy);
        }

        private void TryBuy()
        {
            var added = _toBuy.TryBuyItem(_playerData);
            if (!added)
            {
                Debug.Log($"[{nameof(UButtonBuyGameItem)}] Not enough coins!");
            }
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