using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InitSingalBus();
    }

    private void InitSingalBus()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerDiedSignal>();
        Container.DeclareSignal<GhostDiedSignal>();
    }
}
