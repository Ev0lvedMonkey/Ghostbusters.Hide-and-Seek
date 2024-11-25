using System.Collections.Generic;
using UnityEngine;

public class BlockingSphere : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;

    private HashSet<GameObject> trappedTransformingShoots = new HashSet<GameObject>();
    private const float DestroyTime = 6f;

    private void OnValidate()
    {
        if(_sphereCollider == null)
            _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform enterColiderTransform = other.gameObject.transform.parent;
        if (enterColiderTransform.TryGetComponent(out TransformingShoot transformingShootComponent))
        {
            Debug.LogError($"TransformingShoot {enterColiderTransform.name} entered the sphere");
            trappedTransformingShoots.Add(enterColiderTransform.gameObject);
        }
        else
        {
            Debug.LogError($"Something wrong {enterColiderTransform.name} ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform exitColiderTransform = other.gameObject.transform.parent;
        if (exitColiderTransform == null)
            return;

        if (exitColiderTransform.TryGetComponent(out TransformingShoot transformingShootComponent) 
            && trappedTransformingShoots.Contains(exitColiderTransform.gameObject))
        {
            Debug.LogError($"TransformingShoot {exitColiderTransform.name} tried to exit the sphere but was blocked");
            Vector3 center = transform.position;
            Vector3 ghostPosition = exitColiderTransform.position;

            Vector3 directionFromCenter = (ghostPosition - center).normalized;

            float radius = _sphereCollider.radius - 0.1f; 
            Vector3 boundaryPoint = center + directionFromCenter * radius;

            exitColiderTransform.transform.position = boundaryPoint;
        }
        else
        {
            Debug.LogError($"Something wrong {exitColiderTransform.name} EXIT ");
        }

    }

    private void OnDisable()
    {
        trappedTransformingShoots.Clear();
    }
}
