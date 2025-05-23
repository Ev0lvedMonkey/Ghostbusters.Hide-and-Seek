using UnityEngine;

public class HealthKit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out WeaponFire component))
            return;
        if (!component.TryGetComponent(out CharacterHealthController healthController))
            return;
        if (!healthController.IsNeedHealth())
            return;
        
        healthController.Heal();
        Destroy(gameObject);
    }
}