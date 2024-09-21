using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GhostHealthModel : HealthModel, ISignalBusUser
{
    private SignalBus _signalBus;

    public GhostHealthModel() : base(150)
    {
    }

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        if (IsDead())
        {
            _signalBus.Fire<PlayerDiedSignal>();
            Debug.Log($"Invoke death");
        }
    }
}
