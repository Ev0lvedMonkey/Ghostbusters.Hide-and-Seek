using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMouseInput : MouseInput
{
    [SerializeField] private GhostMover _ghostMover;

    void Update()
    {
        if (!IsOwner) return;

        base.SetMousePos();
        base.UpdateRotationServerRpc(_xAxis, _yAxis);
    }

    protected override void ApplyRotation()
    {
        base.CamRotation();
        if (_ghostMover.IsRotationLocked) return;
        base.BodyRotation();        
    }
}
