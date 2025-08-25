using Internal.Gameplay;
using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private HealthSystem _playerHealth;
        
        public override void InstallBindings()
        {
            // Debug.Log($"[{GetType().Name}] Installing 'Player' binds...");
            Container
                .BindInterfacesAndSelfTo<InputReader>()
                .AsSingle()
                .NonLazy();

            if (_playerHealth == null)
            {
                Debug.LogError($"[{GetType().Name}] PlayerHealth in Player Installer is null");
            }
            
            Container
                .Bind<HealthSystem>()
                .FromInstance(_playerHealth);

            Container.Bind<GameTimer>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}