using Definitions.BulletModificators.Scripts;
using Internal.Core.DataModel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.Buttons
{
    public class UButtonBuyBulletModificator : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;

        [Space] 
        [SerializeField] private int _cost = 10;
        [SerializeField] private BulletModificatorSO _toBuy;

        private PlayerData _playerData;

        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void Awake()
        {
            _buttonToBind.onClick.AddListener(TryBuyModificator);
        }

        private void TryBuyModificator()
        {
            // seems strange to check it in this class. Maybe TODO: ?
            if (_playerData.Coins < _cost)
            {
                Debug.Log($"[{nameof(UButtonBuyBulletModificator)}] Not enough coins!");
                return;
            }

            _playerData.Coins.Value -= _cost;
            _playerData.AddBulletModificatorInInventory(_toBuy);
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