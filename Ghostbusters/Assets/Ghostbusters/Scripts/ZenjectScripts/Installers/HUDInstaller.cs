using UnityEngine;
using Zenject;

public class HUDInstaller : MonoInstaller
{
    [SerializeField] private GhostbusterHealthView _ghostbusterHealthView;
    [SerializeField] private GhostHealthView _ghostHealthView;

    public override void InstallBindings()
    {
        //Container.Bind<GhostbusterHealthModel>().FromNew().AsSingle();
        //Container.Bind<GhostbusterHealthView>().FromInstance(_ghostbusterHealthView);
        //Container.Bind<GhostbusterHealthController>().FromNew().AsSingle();
        //Debug.Log("BUSTER MVC Zenjected");

        //Container.Bind<GhostHealthModel>().FromNew().AsSingle();
        //Container.Bind<GhostHealthView>().FromInstance(_ghostHealthView);
        //Container.Bind<GhostHealthController>().FromNew().AsSingle();
        //Debug.Log("GHOST MVC Zenjected");
    }

}
