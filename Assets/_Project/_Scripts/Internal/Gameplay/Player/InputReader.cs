using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player
{
    public class InputReader : IInitializable, IDisposable
    {
        public event Action<Vector2> OnShootPressed;
        
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
        }

        private void OnShoot(InputAction.CallbackContext context)
        {
            OnShootPressed?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
