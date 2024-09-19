using UnityEngine;
using Zenject;

public class HUDInstaller : MonoInstaller
{
    [SerializeField] private HealthView _healthView;

    public override void InstallBindings()
    {
        Container.Bind<HealthModel>().AsSingle();
        Container.Bind<HealthView>().FromInstance(_healthView);
        Container.Bind<HealthController>().AsSingle();
    }

}
