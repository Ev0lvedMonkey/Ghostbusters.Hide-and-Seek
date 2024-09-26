using Zenject;
using UnityEngine;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private WeponFire _buster;
    [SerializeField] private TransformingShoot _ghost;

    public override void InstallBindings()
    {
        InitSingalBus();
        Container.BindFactory<WeponFire, WeponFireFactory>().FromComponentInNewPrefab(_buster);
        Container.BindFactory<TransformingShoot, TransformingShootFactory>().FromComponentInNewPrefab(_ghost);
    }

    private void InitSingalBus()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerDiedSignal>();
        Container.DeclareSignal<GhostDiedSignal>();
    }
}
