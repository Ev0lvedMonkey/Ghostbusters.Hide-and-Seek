using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class EnvironmentSpawner : NetworkBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public Transform spawnPoint;
        public GameObject prefab;
    }

    [SerializeField] private List<SpawnEntry> spawnEntries = new();

    private readonly List<GameObject> spawnedObjects = new();

    public void SpawnAllClient()
    {
        Debug.LogError($"IsHost 3 {IsHost} ");
        foreach (var entry in spawnEntries)
        {
            if (entry.prefab == null || entry.spawnPoint == null)
                continue;

            GameObject obj = Instantiate(entry.prefab, entry.spawnPoint.position, entry.spawnPoint.rotation);
            NetworkObject netObj = obj.GetComponent<NetworkObject>();

            Debug.LogError($"IsHost 4 {IsHost} ");

            spawnedObjects.Add(obj);
        }
    }
    
    public void SpawnAllHost()
    {
        Debug.LogError($"IsHost 3 {IsHost} ");
        foreach (var entry in spawnEntries)
        {
            if (entry.prefab == null || entry.spawnPoint == null)
                continue;

            GameObject obj = Instantiate(entry.prefab, entry.spawnPoint.position, entry.spawnPoint.rotation);
            NetworkObject netObj = obj.GetComponent<NetworkObject>();

            if (netObj != null && !netObj.IsSpawned)
            {
                netObj.Spawn();
            }
            
            spawnedObjects.Add(obj);
        }
    }

    public void DespawnAll()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                var netObj = obj.GetComponent<NetworkObject>();
                if (netObj != null && netObj.IsSpawned && IsServer)
                {
                    netObj.Despawn();
                }
                else
                {
                    Destroy(obj);
                }
            }
        }

        spawnedObjects.Clear();
    }
}