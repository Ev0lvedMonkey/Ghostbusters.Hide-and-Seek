public class GhostbusterMouseInput : MouseInput
{
    void Update()
    {
        if (!IsOwner) return;

        base.SetMousePos();
        base.UpdateRotationServerRpc(_xAxis, _yAxis);
    }
}
