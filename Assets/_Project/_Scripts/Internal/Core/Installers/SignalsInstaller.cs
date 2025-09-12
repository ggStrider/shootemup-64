using Internal.Core.Signals;
using Zenject;

namespace Internal.Core.Installers
{
    public class SignalsInstaller : MonoInstaller<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container); 
            
            Container.DeclareSignal<AnyEnemyHitInPlayerSignal>().OptionalSubscriber();
            
            Container.DeclareSignal<RealEnemyKilledSignal>().OptionalSubscriber();
            Container.DeclareSignal<AnyEnemyKilledSignal>().OptionalSubscriber();
            Container.DeclareSignal<FakeEnemyKilledSignal>().OptionalSubscriber();

            Container.DeclareSignal<FakeEnemyHitInPlayerSignal>().OptionalSubscriber();
            Container.DeclareSignal<RealEnemyHitInPlayerSignal>().OptionalSubscriber();
            
            Container.DeclareSignal<BackgroundChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<PlayerShootSignal>().OptionalSubscriber();

            Container.DeclareSignal<GameEndSignal>().OptionalSubscriber();
        }
    }
}