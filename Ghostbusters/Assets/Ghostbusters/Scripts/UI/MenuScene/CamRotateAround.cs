using UnityEngine;

public class CamRotateAround : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _waveAmplitude = 2f;
    [SerializeField] private float _waveFrequency = 1f;

    private float _initialHeight;
    private int _rotateDirection;

    private void Awake()
    {
        CursorController.EnableCursor();
    }

    private void OnValidate()
    {
        if (_target == null)
            _target = transform.parent;
    }

    private void Start()
    {
        _initialHeight = transform.position.y - _target.position.y;
        DefinitionOfRotateDirection();
    }


    private void Update()
    {
        transform.RotateAround(_target.position, Vector3.up, (_rotationSpeed * _rotateDirection) * Time.deltaTime);

        float waveOffset = Mathf.Sin(Time.time * _waveFrequency) * _waveAmplitude;
        Vector3 newPosition = transform.position;
        newPosition.y = _target.position.y + _initialHeight + waveOffset;

        transform.position = newPosition;

        transform.LookAt(_target);

    }
    private void DefinitionOfRotateDirection()
    {
        int randomNumber = Random.Range(0, 1);
        if (randomNumber == 0)
            _rotateDirection = -1;
        else
            _rotateDirection = 1;
    }
}
