using UnityEngine;
using UnityEngine.Events;

public class GhostMouseInput : MouseInput
{
    [Header("Ghost Mouse Input Сomponents")]
    [SerializeField] private GhostMover _ghostMover;
    [SerializeField] private DeafaultCamPos _deafaultCamPos;
    
    private readonly UnityEvent OnSetDeafaultStats = new();
    
    private bool _isFirstFrame;

    private const float MinusMouseAngleLimit = 25f;
    private const float PLusMouseAngleLimit = 10f;
    
    private void Start()
    {
        SetMinusAngleLimit(MinusMouseAngleLimit);
        SetPlusAngleLimit(PLusMouseAngleLimit);
    }

    private void Update()
    {
        if (!IsOwner) return;

        base.SetMousePos();
        base.UpdateRotationServerRpc(_xAxis, _yAxis);
    }

    protected override void Init()
    {
        base.Init();
        OnSetDeafaultStats.AddListener(_deafaultCamPos.SetDeafaultStats);
    }

    protected override void ApplyRotation()
    {
        if (_ghostMover.IsRotationLocked)
        {
            OnSetDeafaultStats.Invoke();
            base.CustomCamRotation();
            SetDefaultBodyRotation();

            _isFirstFrame = false;
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
