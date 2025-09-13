using Definitions.BulletModificators.Scripts;
using Internal.Core.DataModel;
using NaughtyAttributes;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.Player
{
    public class USelectModificatorForNextBullet : MonoBehaviour
    {
        [SerializeField] private Button _buttonWhichSelects;
        [SerializeField] private TextMeshProUGUI _amountLabel;
        [SerializeField] private BulletModificatorSO _modificatorToUse;

        [Space] [SerializeField] private Image _modificatorIconPlaceholder;

        [Space] [ReadOnly] 
        [SerializeField] private PlayerData _playerData;
        
        private PlayerShoot _playerShoot;

        [Inject]
        private void Construct(PlayerShoot playerShoot, PlayerData playerData)
        {
            _playerShoot = playerShoot;
            _playerData = playerData;
        }

        private void Awake()
        {
            if (_modificatorToUse == null)
            {
                Debug.LogError($"[{nameof(USelectModificatorForNextBullet)}] Modificator to use == null");
                return;
            }
            
            _buttonWhichSelects.onClick.AddListener(SetToUseModificatorInNextBullet);
            _modificatorIconPlaceholder.sprite = _modificatorToUse.ItemIcon;
            _modificatorIconPlaceholder.color = _modificatorToUse.IconColor;
            
            SetAmountToLabel();
        }

        public void SetToUseModificatorInNextBullet()
        {
            var itemInInventory = _playerData.TryGetBulletModificatorInInventoryBy(_modificatorToUse);
            if (itemInInventory != null)
            {
                if (_playerShoot.TryAddBulletModificatorToNextBullet(_modificatorToUse))
                {
                    _playerData.SubtractBulletModificatorInInventory(_modificatorToUse);
                    SetAmountToLabel(itemInInventory);
                }
            }
        }

        private void SetAmountToLabel(BulletModificatorInInventory itemInInventory = null)
        {
            itemInInventory ??= _playerData.TryGetBulletModificatorInInventoryBy(_modificatorToUse);

            if (itemInInventory == null)
            {
                _amountLabel.text = "0";
            }
            else
            {
                _amountLabel.text = itemInInventory.Amount.ToString();
            }
        }

        public void SetModificatorToSelect(BulletModificatorSO bulletModificator)
        {
            _modificatorToUse = bulletModificator;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (_buttonWhichSelects == null)
            {
                if (TryGetComponent<Button>(out var button)) _buttonWhichSelects = button;
            }
        }
#endif
    }
}