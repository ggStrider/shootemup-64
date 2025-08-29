using System.Collections.Generic;
using Audio;
using Internal.Core.Pools;
using Internal.Core.Signals;
using Shoot;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private BulletBehaviour _bulletPrefab;
        [SerializeField] private Transform _bulletOutPosition;

        private readonly Dictionary<Vector2Int, Quaternion> _directionRotations = new()
        {
            { Vector2Int.right, Quaternion.Euler(0f, 0f, -90f) },   // right
            { Vector2Int.left, Quaternion.Euler(0f, 0f, 90f) },     // left
            { Vector2Int.up, Quaternion.Euler(0f, 0f, 0f) },        // up
            { Vector2Int.down, Quaternion.Euler(0f, 0f, 180f) }     // down
        };

        private InputReader _inputReader;
        private SignalBus _signalBus;
        private BulletPool _bulletsPool;

        [Inject]
        private void Construct(InputReader inputReader, BulletPool bulletsPool, SignalBus signalBus)
        {
            if (inputReader == null)
            {
                Debug.LogError($"[{GetType().Name}] Input reader in constructor is null");
                return;
            }

            _inputReader = inputReader;
            _bulletsPool = bulletsPool;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            if (_inputReader == null)
            {
                Debug.LogError($"[{GetType().Name}] Input reader is null!");
                return;
            }

            _inputReader.OnShootPressed += Shoot;

            if (_bulletPrefab == null)
            {
                Debug.LogError($"[{GetType().Name}] Bullet prefab is null!");
            }

            if (_bulletOutPosition == null)
            {
                Debug.LogError($"[{GetType().Name}] Bullet out position is null!");
            }
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnShootPressed -= Shoot;
            }
        }

        private void Shoot(Vector2 fireDirection)
        {
            // Debug.Log($"[{GetType().Name}] Fire direction: {fireDirection}");

            // between -1 to 1
            var roundedFireDirection = CalculateRoundedFireVector(fireDirection);
            RotatePlayerTowardsFireDirection(roundedFireDirection);
            ShootBullet(roundedFireDirection);

            // Debug.Log($"[{GetType().Name}] Rounded fire direction: {roundedFireDirection}");
        }

        private void ShootBullet(Vector2 roundedFireDirection)
        {
            var bullet = _bulletsPool.Spawn();
            bullet.transform.position = _bulletOutPosition.position;

            bullet.InitializeBeforeShoot(roundedFireDirection);
            _signalBus.Fire(new PlayerShootSignal(bullet));
        }

        private void RotatePlayerTowardsFireDirection(Vector2 roundedFireDirection)
        {
            // transform is a player
            var direction = new Vector2Int(
                Mathf.RoundToInt(roundedFireDirection.x),
                Mathf.RoundToInt(roundedFireDirection.y)
            );

            if (_directionRotations.TryGetValue(direction, out var rotation))
            {
                transform.rotation = rotation;
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}] Unknown fire direction: {direction}");
            }
        }

        private Vector2 CalculateRoundedFireVector(Vector3 fireDirection)
        {
            if (Mathf.Approximately(Mathf.Abs(fireDirection.x), Mathf.Abs(fireDirection.y)))
            {
                return new Vector2(Mathf.Sign(fireDirection.x), 0);
            }

            return fireDirection;
        }
    }
}