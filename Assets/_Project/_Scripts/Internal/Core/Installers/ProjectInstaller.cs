using Audio;
using Internal.Core.Scenes;
using UnityEngine;
using Zenject;

namespace Internal.Core.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private MusicManager _musicManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<MusicManager>()
                .FromComponentInNewPrefab(_musicManagerPrefab)
                .AsSingle()
                .NonLazy();

            Container.Bind<SceneLoader>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}