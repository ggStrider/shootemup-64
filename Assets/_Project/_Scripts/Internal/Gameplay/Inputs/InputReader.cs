using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player
{
    public class InputReader : IInitializable, IDisposable
    {
        public event Action<Vector2> OnShootPressed;

        public event Action OnFirstModificatorSlotPressed;
        public event Action OnSecondModificatorSlotPressed;
        public event Action OnThirdModificatorSlotPressed;
        
        private PlayerMap _playerMap;
        
        public void Initialize()
        {
            _playerMap = new PlayerMap();
            SubscribeButtons();
            _playerMap.Player.Enable();
        }

        private void SubscribeButtons()
        {
            _playerMap.Player.Shoot.performed += OnShoot;

            _playerMap.Player.ModificatorSlot1.performed += OnUseFirstModificatorSlot;
            _playerMap.Player.ModificatorSlot2.performed += OnUseSecondModificatorSlot;
            _playerMap.Player.ModificatorSlot3.performed += OnUseThirdModificatorSlot;
        }
        
        public void Dispose()
        {
            _playerMap.Player.Disable();
            UnsubscribeInGameButtons();
            _playerMap.Dispose();
        }

        public void UnsubscribeInGameButtons()
        {
            _playerMap.Player.Shoot.performed -= OnShoot;
            
            _playerMap.Player.ModificatorSlot1.performed -= OnUseFirstModificatorSlot;
            _playerMap.Player.ModificatorSlot2.performed -= OnUseSecondModificatorSlot;
            _playerMap.Player.ModificatorSlot3.performed -= OnUseThirdModificatorSlot;
        }

        private void OnShoot(InputAction.CallbackContext context)
        {
            OnShootPressed?.Invoke(context.ReadValue<Vector2>());
        }
        
        private void OnUseFirstModificatorSlot(InputAction.CallbackContext context)
        {
            OnFirstModificatorSlotPressed?.Invoke();
        }

        private void OnUseSecondModificatorSlot(InputAction.CallbackContext context)
        {
            OnSecondModificatorSlotPressed?.Invoke();
        }

        private void OnUseThirdModificatorSlot(InputAction.CallbackContext context)
        {
            OnThirdModificatorSlotPressed?.Invoke();
        }
    }
}
