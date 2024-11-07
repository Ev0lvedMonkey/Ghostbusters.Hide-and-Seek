using Unity.Netcode;
using System;
using UnityEngine;

public abstract class MouseInput : NetworkBehaviour
{
    [SerializeField, Range(1f, 5f)] private float _mouseSense;
    [SerializeField] private Transform _camFollowPosition;
    [SerializeField] private Transform _torseObj;

    protected float _xAxis, _yAxis;
    private IRotatable _fullbodyRotation;
    private IRotatable _torseRotation;
    private IRotatable _camRotation;

    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";
    private const float AngleLimit = 80f;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
    }

    [ServerRpc]
    protected virtual void UpdateRotationServerRpc(float xAxis, float yAxis)
    {
        UpdateRotationClientRpc(xAxis, yAxis);
    }

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

    protected virtual void ApplyRotation()
    {
        TorseRotation();
        FullBodyRotation();
        CamRotation();
    }

    protected virtual void CamRotation()
    {
        _camRotation.Rotate(_xAxis, _yAxis);
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

    private void Init()
    {
        _fullbodyRotation = new FullBodyRotation(transform);
        _camRotation = new CamRotation(_camFollowPosition);
        _torseRotation = new TorseRotation(_torseObj);
    }

    protected virtual void SetMousePos()
    {
        _xAxis += Input.GetAxisRaw(MouseX);
        _yAxis -= Input.GetAxisRaw(MouseY);
        _yAxis = Mathf.Clamp(_yAxis, -AngleLimit, AngleLimit);
    }
}
