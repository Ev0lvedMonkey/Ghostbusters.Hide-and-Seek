using Mirror;
using System;
using UnityEngine;

public class MouseInput : NetworkBehaviour
{
    [SerializeField, Range(1f, 5f)] private float _mouseSense;
    [SerializeField] private Transform _camFollowPosition;
    [SerializeField] private Transform _torseObj;

    private float _xAxis, _yAxis;
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
        if(!isLocalPlayer) return;

        SetMousePos();
    }

    private void LateUpdate()
    {
        _fullbodyRotation.Rotate(_xAxis, _yAxis);
        _torseRotation.Rotate(_xAxis, _yAxis);
        _camRotation.Rotate(_xAxis, _yAxis);
    }

    private void Init()
    {
        _fullbodyRotation = new FullBodyRotation(transform);
        _camRotation = new CamRotation(_camFollowPosition);
        _torseRotation = new TorseRotation(_torseObj);
    }

    private void SetMousePos()
    {
        _xAxis += Input.GetAxisRaw(MouseX);
        _yAxis -= Input.GetAxisRaw(MouseY);
        _yAxis = Mathf.Clamp(_yAxis, -AngleLimit, AngleLimit);
    }
}
