using Internal.Gameplay.Managers;
using UnityEngine;
using Zenject;

namespace Internal.Core.Installers
{
    public class UInstaller : MonoInstaller
    {
        [SerializeField] private UStatisticsOfRound _uStatisticsOfRound;
        
        public override void InstallBindings()
        {
            Container.Bind<UStatisticsOfRound>()
                .FromInstance(_uStatisticsOfRound)
                .AsSingle()
                .NonLazy();
        }
    }
}