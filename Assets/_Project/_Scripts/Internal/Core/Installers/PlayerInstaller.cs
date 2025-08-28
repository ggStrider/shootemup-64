using Audio;
using Internal.Gameplay;
using Internal.Gameplay.EntitiesShared;
using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private AudioEffectsManager _audioEffectsManager;
        [SerializeField] private Camera _playerCamera;
        
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
                .Bind<PlayerHealth>()
                .FromInstance(_playerHealth);

            Container.Bind<GameTimer>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();

            Container.Bind<AudioEffectsManager>()
                .FromInstance(_audioEffectsManager)
                .AsSingle()
                .NonLazy();

            Container.Bind<Camera>()
                .FromInstance(_playerCamera)
                .AsSingle()
                .NonLazy();
        }
    }
}