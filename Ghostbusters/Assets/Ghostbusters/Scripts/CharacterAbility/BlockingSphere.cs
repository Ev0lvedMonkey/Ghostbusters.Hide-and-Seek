using System.Collections.Generic;
using UnityEngine;

public class BlockingSphere : MonoBehaviour
{
    private const float DestroyTime = 6f;

    [SerializeField] private SphereCollider _sphereCollider;

    private HashSet<GameObject> _trappedTransformingShoots = new();

    private void OnValidate()
    {
        if (_sphereCollider == null)
            _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform enterColiderTransform = other.gameObject.transform.parent;
        if (enterColiderTransform == null)
            return;

        if (!enterColiderTransform.TryGetComponent(out TransformingShoot transformingShootComponent))
            return;
        _trappedTransformingShoots.Add(enterColiderTransform.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform exitColiderTransform = other.gameObject.transform.parent;
        if (exitColiderTransform == null)
            return;

        if (!exitColiderTransform.TryGetComponent(out TransformingShoot transformingShootComponent)
            && _trappedTransformingShoots.Contains(exitColiderTransform.gameObject))
            return;

        Vector3 center = transform.position;
        Vector3 ghostPosition = exitColiderTransform.position;

        Vector3 directionFromCenter = (ghostPosition - center).normalized;

        float radius = _sphereCollider.radius - 0.1f;
        Vector3 boundaryPoint = center + directionFromCenter * radius;

        exitColiderTransform.transform.position = boundaryPoint;
    }

    private void OnDisable()
    {
        _trappedTransformingShoots.Clear();
    }
}