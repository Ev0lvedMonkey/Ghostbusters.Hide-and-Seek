using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ISignalBusUser 
{
    public void Construct(SignalBus signalBus);
}
