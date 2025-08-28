using Internal.Core.Signals;
using Zenject;

namespace Internal.Core.Installers
{
    public class SignalsInstaller : MonoInstaller<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container); 
            
            Container.DeclareSignal<EnemyHitInPlayerSignal>();
            Container.DeclareSignal<EnemyDieSignal>();
        }
    }
}