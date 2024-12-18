using UnityEngine;

public class CamRotateAround : MonoBehaviour
{
    private float _initialHeight;
    private int _rotateDirection;

    [SerializeField] private Transform _target;
    [SerializeField] private CameraRotationProperties _cameraRotationProperties;

    private void Update()=>
        RotateCam();
    
    public void DefinitionOfRotateDirection()
    {
        int randomNumber = Random.Range(0, 1);
        if (randomNumber == 0)
            _rotateDirection = -1;
        else
            _rotateDirection = 1;
    }

    public void InitHeight() =>
        _initialHeight = transform.position.y - _target.position.y;

    private void RotateCam()
    {
        transform.RotateAround(_target.position, Vector3.up, (_cameraRotationProperties.RotationSpeed * _rotateDirection) * Time.deltaTime);

        float waveOffset = Mathf.Sin(Time.time * _cameraRotationProperties.WaveFrequency) * _cameraRotationProperties.WaveAmplitude;
        Vector3 newPosition = transform.position;
        newPosition.y = _target.position.y + _initialHeight + waveOffset;

        transform.position = newPosition;

        transform.LookAt(_target);
    }
}
