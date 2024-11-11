using UnityEngine;

public class GhostbusterMouseInput : MouseInput
{
    private void Start()
    {
        Debug.Log($"Set nouse limits buster");
        SetMinusAngleLimit(25);
        SetPlusAngleLimit(25);
    }

    void Update()
    {
        if (!IsOwner) return;

        base.SetMousePos();
        base.UpdateRotationServerRpc(_xAxis, _yAxis);
    }
}
