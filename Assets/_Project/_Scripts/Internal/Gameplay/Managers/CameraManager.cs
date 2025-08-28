using DG.Tweening;
using Internal.Core.Signals;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// Injects by <see cref="Installers.PlayerInstaller"/>
        /// </summary>
        [Header("Injects by Zenject")]
        [SerializeField, ReadOnly] private Camera _mainCamera;
        
        private SignalBus _signalBus;

        [Inject]
        private void Construct(Camera playerCamera, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _mainCamera = playerCamera;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyDieSignal>(ShakeCamera);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<EnemyDieSignal>(ShakeCamera);
        }

        private void ShakeCamera()
        {
            _mainCamera.DOShakePosition(0.1f, 0.25f);
        }
    }
}