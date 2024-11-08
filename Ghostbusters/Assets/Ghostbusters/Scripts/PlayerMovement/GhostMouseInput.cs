using UnityEngine;
using UnityEngine.Events;

public class GhostMouseInput : MouseInput
{
    [SerializeField] private GhostMover _ghostMover;
    [SerializeField] private UnityEvent OnSetDeafaultStats; 
    private bool _isFirstFrame;

    void Update()
    {
        if (!IsOwner) return;

        base.SetMousePos();
        base.UpdateRotationServerRpc(_xAxis, _yAxis);
    }

    protected override void ApplyRotation()
    {
        if (_ghostMover.IsRotationLocked)
        {
            OnSetDeafaultStats.Invoke();
            base.CustomCamRotation();
            _isFirstFrame = false;
            return;
        }
        else
        {
            if (_isFirstFrame == false)
            {
                OnSetDeafaultStats.Invoke();
                _isFirstFrame = true;
            }
            base.ApplyRotation();
        }
    }
}
