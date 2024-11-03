using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class SpawnNetworkPlayer : NetworkBehaviour
{
    public static SpawnNetworkPlayer LocalInstance { get; private set; }

    private UnityEvent OnSettedSpawnPos = new();

    [SerializeField] private List<Vector3> spawnPositionList;

    private Vector3 spawnPosition;

    private void Awake()
    {
        OnSettedSpawnPos.AddListener(() =>
        {
            transform.position = spawnPosition;
            Debug.Log($"{spawnPosition} --- {transform.position}");
        });
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
            int playerIndex = MultiplayerStorage.Instance.GetPlayerDataIndexFromClientId(MultiplayerStorage.Instance.GetPlayerData().clientId);
            spawnPosition = spawnPositionList[playerIndex];
            Debug.Log($"{playerIndex} ----- {spawnPosition}");
            OnSettedSpawnPos.Invoke();
        }
    }
}
