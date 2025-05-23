public class GhostbusterMouseInput : MouseInput
{
    private const float MouseAngleLimits = 25f;
    
    private void Start()
    {
        SetMinusAngleLimit(-MouseAngleLimits);
        SetPlusAngleLimit(MouseAngleLimits);
    }

    private void Update()
    {
        if (!IsOwner) return;

        base.SetMousePos();
        base.UpdateRotationServerRpc(_xAxis, _yAxis);
    }
}
