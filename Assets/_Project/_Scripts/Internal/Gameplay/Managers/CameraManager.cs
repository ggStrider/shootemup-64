using DG.Tweening;
using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
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

        private PlayerHealth _playerHealth;

        [Inject]
        private void Construct(Camera playerCamera, SignalBus signalBus, PlayerHealth playerHealth)
        {
            _signalBus = signalBus;
            _mainCamera = playerCamera;
            _playerHealth = playerHealth;
        }

        private void Awake()
        {
            _startCameraPosition = _mainCamera.transform.position;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<RealEnemyKilledSignal>(ShakeCamera);
            _signalBus.Subscribe<FakeEnemyKilledSignal>(ShakeCamera);

            _playerHealth.OnDamageTaken += _ => ShakeCamera();
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<RealEnemyKilledSignal>(ShakeCamera);
            _signalBus.TryUnsubscribe<FakeEnemyKilledSignal>(ShakeCamera);
            
            _playerHealth.OnDamageTaken -= _ => ShakeCamera();
        }

        private void ShakeCamera()
        {
            _mainCamera.DOKill();
            _mainCamera.DOShakePosition(_shakeDuration, _shakeStrength)
                .OnComplete(() => _mainCamera.transform.position = _startCameraPosition);
        }
    }
}