using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Internal.Core.Signals;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay.Player
{
    public class PlayerMoveOffsetOnShoot : MonoBehaviour
    {
        [SerializeField] private float _tweenDuration = 0.1f;
        [Space] [SerializeField] private float _offsetDelta = 0.5f;

        private Vector3 _spawnPosition;
        private Dictionary<Vector2Int, Vector3> _offsetWhenShoot;

        private CancellationTokenSource _waitToResetCts;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _spawnPosition = transform.position;
            InitializeDictionary();
        }
        
        private void InitializeDictionary()
        {
            _offsetWhenShoot = new()
            {
                { Vector2Int.right, new(-_offsetDelta, 0, 0) }, // right
                { Vector2Int.left, new(_offsetDelta, 0, 0) }, // left
                { Vector2Int.up, new(0, -_offsetDelta, 0) }, // up
                { Vector2Int.down, new(0, _offsetDelta, 0) } // down
            };
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerShootSignal>(DoOffset);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<PlayerShootSignal>(DoOffset);
        }
        
        private void DoOffset(PlayerShootSignal signal)
        {
            transform.DOKill();
            transform.position = _spawnPosition;

            if (_offsetWhenShoot.TryGetValue(signal.Direction, out var value))
            {
                // 2 cuz its move back and then to start pos
                transform.DOMove(value, _tweenDuration).SetLoops(2, LoopType.Yoyo);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            InitializeDictionary();
        }
#endif
    }
}