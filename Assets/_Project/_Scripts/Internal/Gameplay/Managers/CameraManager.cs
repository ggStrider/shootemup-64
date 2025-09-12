using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Scenes.Cards;
using DG.Tweening;
using Internal.Core.Scenes;
using Internal.Core.Signals;
using Internal.Gameplay.EntitiesShared;
using NaughtyAttributes;
using Tools;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float _shakeDuration = 0.1f;
        [SerializeField] private float _shakeStrength = 0.25f;

        [Space, Header("Bass Shake")] 
        [SerializeField] private Transform _bassShakeTransform;
        [SerializeField] private float _pulseStrength = 0.2f;
        [SerializeField] private float _pulseDuration = 0.1f;
        [SerializeField] private int _punchVibrato = 5;
        [SerializeField] private float _zoomStrength = 0.2f;
        [SerializeField] private float _zoomDuration = 0.1f;

        private float _defaultOrthoSize;
        private Vector3 _defaultBassShakePosition;

        /// <summary>
        /// Injects by <see cref="Installers.PlayerInstaller"/>
        /// </summary>
        [Header("Injects by Zenject")] [SerializeField, ReadOnly]
        private Camera _mainCamera;

        private Vector3 _startCameraPosition;
        private SignalBus _signalBus;

        private PlayerHealth _playerHealth;
        private SceneCard _sceneCard;

        private CancellationTokenSource _cameraBassShakeCts;

        [Inject]
        private void Construct(Camera playerCamera, SignalBus signalBus, PlayerHealth playerHealth,
            SceneCardHolder sceneCardHolder)
        {
            _signalBus = signalBus;
            _mainCamera = playerCamera;
            _playerHealth = playerHealth;

            _sceneCard = sceneCardHolder.CurrentSceneCard;
        }

        private void Awake()
        {
            _startCameraPosition = _mainCamera.transform.position;

            _defaultOrthoSize = _mainCamera.orthographicSize;
            _defaultBassShakePosition = _bassShakeTransform.localPosition;
        }

        private void OnDestroy()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _cameraBassShakeCts);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<RealEnemyKilledSignal>(ShakeCamera);
            _signalBus.Subscribe<FakeEnemyKilledSignal>(ShakeCamera);

            _playerHealth.OnDamageTaken += _ => ShakeCamera();
        }

        public void InitializeCameraBassShake()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _cameraBassShakeCts, true);
            ProcessCameraBassShake(_cameraBassShakeCts.Token).Forget();
        }

        private async UniTaskVoid ProcessCameraBassShake(CancellationToken token)
        {
            try
            {
                for (var i = 0; i < _sceneCard.CameraBassShakeWaves.Count; i++)
                {
                    BassShakeCamera();
                    await UniTask.WaitForSeconds(_sceneCard.CameraBassShakeWaves[i], cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<RealEnemyKilledSignal>(ShakeCamera);
            _signalBus.TryUnsubscribe<FakeEnemyKilledSignal>(ShakeCamera);

            _playerHealth.OnDamageTaken -= _ => ShakeCamera();
        }

        [Button]
        private void ShakeCamera()
        {
            _mainCamera.DOKill();
            _mainCamera.DOShakePosition(_shakeDuration, _shakeStrength)
                .OnComplete(() => _mainCamera.transform.position = _startCameraPosition);
        }

        [Button]
        private void BassShakeCamera()
        {
            _mainCamera.DOKill();
            _bassShakeTransform.DOKill();

            _mainCamera.DOOrthoSize(_defaultOrthoSize - _zoomStrength, _zoomDuration)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => _mainCamera.orthographicSize = _defaultOrthoSize);

            _bassShakeTransform.DOPunchPosition(
                new Vector3(0, -_pulseStrength, 0),
                _pulseDuration,
                _punchVibrato,
                0.5f
            ).OnComplete(() => _bassShakeTransform.localPosition = _defaultBassShakePosition);
        }
    }
}