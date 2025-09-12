using Player;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.UI.Player
{
    public class USelectModificatorByBind : MonoBehaviour
    {
        [SerializeField] private USelectModificatorForNextBullet _firstSlot;
        [SerializeField] private USelectModificatorForNextBullet _secondSlot;
        [SerializeField] private USelectModificatorForNextBullet _thirdSlot;
        
        private InputReader _inputReader;
        
        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void OnEnable()
        {
            _inputReader.OnFirstModificatorSlotPressed += UseFirstSlot;
            _inputReader.OnSecondModificatorSlotPressed += UseSecondSlot;
            _inputReader.OnThirdModificatorSlotPressed += UseThirdSlot;
        }
        
        private void UseFirstSlot()
        {
            _firstSlot.SetToUseModificatorInNextBullet();
        }
        
        private void UseSecondSlot()
        {
            _secondSlot.SetToUseModificatorInNextBullet();
        }
        
        private void UseThirdSlot()
        {
            _thirdSlot.SetToUseModificatorInNextBullet();
        }
    }
}