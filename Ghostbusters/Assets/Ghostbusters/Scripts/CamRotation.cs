
using UnityEngine;

public class CamRotation : IRotatable
{
    private readonly Transform _cameraTransform;

    public CamRotation(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public void Rotate(float xAxis, float yAxis)
    {
        _cameraTransform.localEulerAngles = new Vector3(yAxis, _cameraTransform.localEulerAngles.y, _cameraTransform.localEulerAngles.z);
    }
}
