using UnityEngine;
using UnityEngine.UI;

public class DecideTransformingObject : MonoBehaviour
{
    [SerializeField, Range(1f, 250f)] protected float _maxRayDistance;
    [SerializeField] protected Image _crosshairImage;
    
    private void Update()
    {
        Transform firePosition = ServiceLocator.Current.Get<FirePositionService>().FirePosition;

        Ray ray = new(firePosition.position, firePosition.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance))
        {
            if (!hit.collider.TryGetComponent<TransformableObject>(out var transformableObject))
            {
                _crosshairImage.color = Color.red;
            }
            else
                _crosshairImage.color = Color.cyan;

        }

    }
}
