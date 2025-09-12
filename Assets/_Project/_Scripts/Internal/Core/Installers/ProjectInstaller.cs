using Audio;
using Definitions.Scenes.Cards;
using Internal.Core.DataModel;
using Internal.Core.Scenes;
using UnityEngine;
using Zenject;

namespace Internal.Core.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private MusicManager _musicManagerPrefab;
        [SerializeField] private SceneCardHolder _sceneCardHolder;
        [SerializeField] private SceneCard _startSceneCard;
        
        [Space]
        [SerializeField] private PlayerData _playerData;

        public override void InstallBindings()
        {
            Container.Bind<SceneCardHolder>()
                .FromInstance(_sceneCardHolder)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<MusicManager>()
                .FromComponentInNewPrefab(_musicManagerPrefab)
                .AsSingle()
                .NonLazy();

            Container.Bind<SceneLoader>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            InitializeSaveData();
        }

        private void InitializeSaveData()
        {
            _playerData = SaveSystem.Load();
            
            Container.BindInterfacesAndSelfTo<PlayerData>()
                .FromInstance(_playerData)
                .AsSingle()
                .NonLazy();
        }
    }
}