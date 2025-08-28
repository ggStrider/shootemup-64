using DG.Tweening;
using Internal.Core.Signals;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float _shakeDuration = 0.1f;
        [SerializeField] private float _shakeStrength = 0.25f;
        
        /// <summary>
        /// Injects by <see cref="Installers.PlayerInstaller"/>
        /// </summary>
        [Header("Injects by Zenject")]
        [SerializeField, ReadOnly] private Camera _mainCamera;

        private Vector3 _startCameraPosition;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(Camera playerCamera, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _mainCamera = playerCamera;
        }

        private void Awake()
        {
            _startCameraPosition = _mainCamera.transform.position;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyDieSignal>(ShakeCamera);
            _signalBus.Subscribe<FakeEnemyDieSignal>(ShakeCamera);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<EnemyDieSignal>(ShakeCamera);
            _signalBus.TryUnsubscribe<FakeEnemyDieSignal>(ShakeCamera);
        }

        private void ShakeCamera()
        {
            _mainCamera.DOKill();
            _mainCamera.DOShakePosition(_shakeDuration, _shakeStrength)
                .OnComplete(() => _mainCamera.transform.position = _startCameraPosition);
        }
    }
}