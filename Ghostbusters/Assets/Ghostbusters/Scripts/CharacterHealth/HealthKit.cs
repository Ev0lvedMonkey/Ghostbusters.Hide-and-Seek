using Unity.Netcode;
using UnityEngine;

public class HealthKit : NetworkBehaviour
{
    private NetworkObject networkObject;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsHost || IsServer || IsOwnedByServer)
        {
            networkObject = this.gameObject.GetComponent<NetworkObject>();
            if (!networkObject.IsSpawned)
                networkObject.Spawn(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out WeaponFire component))
        {
            Debug.Log($"Don`t have WeaponFire ib {other.gameObject.name}");
            return;
        }

        if (!component.TryGetComponent(out BusterHealthController healthController))
        {
            Debug.Log($"Don`t have BusterHealthController ib {other.gameObject.name}");
            return;
        }
        if (!healthController.IsNeedHealth())
            return;

        healthController.HealServerRpc();
        DestroyServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyServerRPC()
    {
        if (IsHost || IsServer || IsOwnedByServer)
            networkObject.Despawn(true);
    }
}