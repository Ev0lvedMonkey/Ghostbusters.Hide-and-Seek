using UnityEngine;

public class CamRotateAround : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _waveAmplitude = 2f;
    [SerializeField] private float _waveFrequency = 1f;

    private float initialHeight;

    private void OnValidate()
    {
        if (_target == null)
            _target = transform.parent;
    }

    private void Start()
    {
        initialHeight = transform.position.y - _target.position.y;
    }

    private void Update()
    {
        transform.RotateAround(_target.position, Vector3.up, _rotationSpeed * Time.deltaTime);

        float waveOffset = Mathf.Sin(Time.time * _waveFrequency) * _waveAmplitude;
        Vector3 newPosition = transform.position;
        newPosition.y = _target.position.y + initialHeight + waveOffset;

        transform.position = newPosition;

        transform.LookAt(_target);

    }
}
