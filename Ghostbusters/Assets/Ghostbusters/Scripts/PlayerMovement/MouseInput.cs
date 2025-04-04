using Unity.Netcode;
using UnityEngine;
using static System.Single;

public abstract class MouseInput : NetworkBehaviour
{
    [SerializeField] private MouseSenceConfiguration _mouseSenseConfig;
    [SerializeField] private Transform _camFollowPosition;
    [SerializeField] private Transform _torseObj;

    protected float _xAxis, _yAxis;
    private IRotatable _fullbodyRotation;
    private IRotatable _torseRotation;
    private CamRotation _camRotation;

    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";
    private const float AngleLimit = 25f;

    private const float MinMinusAngleLimit = 0;
    private const float MaxMinusAngleLimit = -AngleLimit;
    private const float MinPlusAngleLimit = 0f;
    private const float MaxPlusAngleLimit = AngleLimit;

    protected float MinusAngleLimit { get; private set; }
    protected float PlusAngleLimit { get; private set; }

    private void Awake()
    {
        Init();
    }

    [ServerRpc]
    protected virtual void UpdateRotationServerRpc(float xAxis, float yAxis) =>
        UpdateRotationClientRpc(xAxis, yAxis);


    [ClientRpc]
    private void UpdateRotationClientRpc(float xAxis, float yAxis)
    {
        if (IsOwner) return;

        _xAxis = xAxis;
        _yAxis = yAxis;
        ApplyRotation();
    }

    private void LateUpdate()
    {
        if (IsOwner)
        {
            ApplyRotation();
        }
    }

    protected void SetDefaultBodyRotation()
    {
        if (IsOwner)
        {
            transform.eulerAngles = new Vector3(0, _torseObj.eulerAngles.y, 0);
        }
    }

    protected void SetMinusAngleLimit(float angle)
    {
        angle = Mathf.Clamp(angle, MinMinusAngleLimit, MaxMinusAngleLimit);
        MinusAngleLimit = angle;
    }

    protected void SetPlusAngleLimit(float angle)
    {
        angle = Mathf.Clamp(angle, MinPlusAngleLimit, MaxPlusAngleLimit);
        PlusAngleLimit = angle;
    }

    protected virtual void ApplyRotation()
    {
        TorseRotation();
        FullBodyRotation();
        CamRotation();
    }

    protected virtual void CamRotation()
    {
        _camRotation.Rotate(_xAxis, _yAxis);
        _camFollowPosition.transform.position
            = new Vector3(_camFollowPosition.transform.position.x, Mathf.Clamp(_camFollowPosition.transform.position.y, 0.85f, MaxValue),_camFollowPosition.transform.position.z);
    }

    protected virtual void CustomCamRotation()
    {
        _camRotation.CustomRotate(_xAxis, _yAxis);
    }

    protected virtual void TorseRotation()
    {
        _fullbodyRotation.Rotate(_xAxis, _yAxis);
        _torseRotation.Rotate(_xAxis, _yAxis);
    }

    protected virtual void FullBodyRotation()
    {
        _fullbodyRotation.Rotate(_xAxis, _yAxis);
        _torseRotation.Rotate(_xAxis, _yAxis);
    }

    protected virtual void SetMousePos()
    {
        _xAxis += Input.GetAxisRaw(MouseX) * _mouseSenseConfig.GetMouseSense();
        _yAxis -= Input.GetAxisRaw(MouseY) * _mouseSenseConfig.GetMouseSense();
        _yAxis = Mathf.Clamp(_yAxis, MinusAngleLimit, PlusAngleLimit);
    }

    private void Init()
    {
        _fullbodyRotation = new FullBodyRotation(transform);
        _camRotation = new CamRotation(_camFollowPosition);
        _torseRotation = new TorseRotation(_torseObj);
    }

}
