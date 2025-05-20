using Audio;
using Shoot;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private BulletBehaviour _bulletPrefab;
        [SerializeField] private Transform _bulletOutPosition;

        [Space] [SerializeField] private AudioPlayerAdvanced _audioPlayer;
        
        private InputReader _inputReader;
        
        [Inject]
        private void Construct(InputReader inputReader)
        {
            if (inputReader == null)
            {
                Debug.LogError($"[{GetType().Name}] Input reader in constructor is null");
                return;
            }
            _inputReader = inputReader;
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
                return;
            }

            if (_bulletOutPosition == null)
            {
                Debug.LogError($"[{GetType().Name}] Bullet out position is null!");
                return;
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
            var bullet = Instantiate(_bulletPrefab, _bulletOutPosition.position, Quaternion.identity);
            bullet.Initialize(roundedFireDirection);

            _audioPlayer?.PlayShotOfRandomSound();
        }

        private void RotatePlayerTowardsFireDirection(Vector2 roundedFireDirection)
        {
            // transform is a player
            if (roundedFireDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0, -90);
            }
            else if (roundedFireDirection.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0, 90);
            }
            else if (roundedFireDirection.y > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0, 180);
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