using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkPlayer : NetworkBehaviour
{
    public static SpawnNetworkPlayer LocalInstance { get; private set; }

    [SerializeField] private List<Vector3> spawnPositionList;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
        int playerIndex = MultiplayerStorage.Instance.GetPlayerDataIndexFromClientId(MultiplayerStorage.Instance.GetPlayerData().clientId);
        transform.position = spawnPositionList[playerIndex];
        Debug.Log($"{playerIndex} ----- {spawnPositionList[playerIndex]}");
    }
}
