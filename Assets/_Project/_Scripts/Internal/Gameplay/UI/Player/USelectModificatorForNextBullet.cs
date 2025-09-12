using Definitions.BulletModificators.Scripts;
using Internal.Core.DataModel;
using NaughtyAttributes;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.Player
{
    public class USelectModificatorForNextBullet : MonoBehaviour
    {
        [SerializeField] private Button _buttonWhichSelects;
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
            _modificatorIconPlaceholder.sprite = _modificatorToUse.BulletModificatorUI.ModificatorIcon;
            _modificatorIconPlaceholder.color = _modificatorToUse.BulletModificatorUI.IconColor;
        }

        private void SetToUseModificatorInNextBullet()
        {
            if (_playerData.TryGetBulletModificatorInInventoryBy(_modificatorToUse) != null)
            {
                if (_playerShoot.TryAddBulletModificatorToNextBullet(_modificatorToUse))
                {
                    _playerData.SubtractBulletModificatorInInventory(_modificatorToUse);
                }
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