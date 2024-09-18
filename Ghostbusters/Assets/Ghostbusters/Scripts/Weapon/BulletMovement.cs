using UnityEngine;
using Cinemachine;
using Org.BouncyCastle.Math.EC;

[RequireComponent(typeof(Rigidbody))]
public class BulletMovement : MonoBehaviour
{
    [SerializeField, Range(0.02f, 3f)] private float _bulletLifeTime;
    [SerializeField] private int _bulletVelocity;
    [SerializeField] private Rigidbody _rb;

    private const float RotateSpeed = 15f;

    private void OnValidate()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, _bulletLifeTime);
        Transform cameraTransform = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera.transform;
        Vector3 shootDirection = cameraTransform.forward;
        _rb.AddForce(shootDirection * _bulletVelocity, ForceMode.Impulse);
    }

    private void Update()
    {
        transform.Rotate(RotateSpeed, 0,0);
    }


}
