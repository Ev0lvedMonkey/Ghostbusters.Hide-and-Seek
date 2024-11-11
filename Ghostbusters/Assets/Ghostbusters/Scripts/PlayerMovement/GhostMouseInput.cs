using UnityEngine;
using UnityEngine.Events;

public class GhostMouseInput : MouseInput
{
    [SerializeField] private GhostMover _ghostMover;
    [SerializeField] private UnityEvent OnSetDeafaultStats;
    private bool _isFirstFrame;

    private void Start()
    {
        Debug.Log($"Set nouse limits ghost");
        SetMinusAngleLimit(25);
        SetPlusAngleLimit(10);
    }

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
            Debug.Log($"is locked");
            OnSetDeafaultStats.Invoke();
            base.CustomCamRotation();
                SetDefaultBodyRotation();

            _isFirstFrame = false;
            return;
        }
        else
        {
            Debug.Log($"unlock !!!!!!!!");
            if (_isFirstFrame == false)
            {
                OnSetDeafaultStats.Invoke();
                _isFirstFrame = true;
            }
            base.ApplyRotation();
        }
    }
}
