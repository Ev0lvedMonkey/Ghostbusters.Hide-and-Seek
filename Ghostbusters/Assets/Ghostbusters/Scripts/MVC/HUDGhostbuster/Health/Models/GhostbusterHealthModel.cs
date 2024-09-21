using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;
using Zenject;

public class GhostbusterHealthModel : HealthModel, ISignalBusUser
{
    private SignalBus _signalBus;

    public GhostbusterHealthModel() : base(100)
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
