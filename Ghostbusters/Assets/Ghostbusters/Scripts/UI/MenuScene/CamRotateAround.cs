using UnityEngine;

public class CamRotateAround : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private CameraRotationProperties _cameraRotationProperties;

    private float _initialHeight;
    private int _rotateDirection;
    private Transform _camTransform;

    private void Update()
    {
        RotateCam();
    }
    
    public void DefinitionOfRotateDirection()
    {
        int randomNumber = Random.Range(0, 2);
        _rotateDirection = randomNumber == 0 ? -1 : 1;
    }

    public void Init()
    {
        _camTransform = this.transform;
        _initialHeight = transform.position.y - _target.position.y;
    }

    private void RotateCam()
    {
        _camTransform.RotateAround(_target.position, Vector3.up, (_cameraRotationProperties.RotationSpeed * _rotateDirection) * Time.deltaTime);

        float waveOffset = Mathf.Sin(Time.time * _cameraRotationProperties.WaveFrequency) * _cameraRotationProperties.WaveAmplitude;
        Vector3 newPosition = _camTransform.position;
        newPosition.y = _target.position.y + _initialHeight + waveOffset;

        _camTransform.position = newPosition;

        _camTransform.LookAt(_target);
    }
}
