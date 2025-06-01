using Unity.Netcode;
using UnityEngine;

public class HealthKit : NetworkBehaviour
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