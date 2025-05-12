using UnityEngine;
using UnityEngine.UI;

public class DecideTransformingObject : MonoBehaviour
{
    private const float MaxRayDistance =  250f;
    private readonly Color TransformableObjectColor = Color.red;
    private readonly Color SimpleObjectColor = Color.cyan;

    [SerializeField] protected Image _crosshairImage;

    private void Update()
    {
        Transform firePosition = ServiceLocator.Current.Get<FirePositionService>().FirePosition;

        Ray ray = new(firePosition.position, firePosition.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance))
        {
            if (!hit.collider.TryGetComponent<TransformableObject>(out var transformableObject))
            {
                _crosshairImage.color = TransformableObjectColor;
            }
            else
                _crosshairImage.color = SimpleObjectColor;
        }
    }
}
