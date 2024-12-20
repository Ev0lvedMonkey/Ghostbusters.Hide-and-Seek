using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkPlayer : NetworkBehaviour
{
    public static SpawnNetworkPlayer LocalInstance { get; private set; }

    [SerializeField] private List<Vector3> spawnPositionList;

    public override void OnNetworkSpawn()
    {
        try
        {
            if (!IsOwner)
                return;

            LocalInstance = this;

            ulong clientID = MultiplayerStorage.Instance.GetPlayerData().clientId;
            int playerIndex = MultiplayerStorage.Instance.GetPlayerDataIndexFromClientId(clientID);
            transform.position = spawnPositionList[playerIndex];
        }
        catch { Debug.LogError("Bug here"); }
    }
}
