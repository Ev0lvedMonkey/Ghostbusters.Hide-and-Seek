using UnityEngine;

public class HealthKit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out WeaponFire component))
            return;
        if (component.TryGetComponent(out CharacterHealthControllerTemp healthController))
        {
            healthController.Heal();
            Destroy(gameObject);
        }
    }
}
