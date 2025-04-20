using UnityEngine;

public class CamRotation 
{
    private readonly Transform _cameraTransform;
    
    public CamRotation(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public void Rotate(float xAxis, float yAxis)
    {
        if (ServiceLocator.Current.Get<GameOverWinUI>().IsOpened())
            return;
        _cameraTransform.localEulerAngles = new Vector3(yAxis, _cameraTransform.localEulerAngles.y, _cameraTransform.localEulerAngles.z);
    }

    public void CustomRotate(float xAxis, float yAxis)
    {
        if (ServiceLocator.Current.Get<GameOverWinUI>().IsOpened())
            return;
        _cameraTransform.localEulerAngles = new Vector3(_cameraTransform.localEulerAngles.x, xAxis, _cameraTransform.localEulerAngles.z);
    }
}
