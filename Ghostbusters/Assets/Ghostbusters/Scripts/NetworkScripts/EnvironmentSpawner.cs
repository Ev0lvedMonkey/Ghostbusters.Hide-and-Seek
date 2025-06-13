using System;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SpawnEntry
{
    public Transform spawnPoint;
    public GameObject prefab;
}

public class EnvironmentSpawner : NetworkBehaviour
{
    private readonly List<GameObject> spawnedObjects = new();

    [SerializeField] private List<SpawnEntry> spawnEntries = new();

    private bool _enviromentSpawned;

    public void SpawnAllClient()
    {
        if (_enviromentSpawned)
            return;
        foreach (var entry in spawnEntries)
        {
            if (entry.prefab == null || entry.spawnPoint == null)
                continue;

            GameObject obj = Instantiate(entry.prefab, entry.spawnPoint.position, entry.spawnPoint.rotation);

            spawnedObjects.Add(obj);
        }

        _enviromentSpawned = true;
    }

    public void SpawnAllHost()
    {
        if (_enviromentSpawned)
            return;
        foreach (SpawnEntry entry in spawnEntries)
        {
            if (entry.prefab == null || entry.spawnPoint == null)
                continue;

            GameObject obj = 
                Instantiate(entry.prefab, entry.spawnPoint.position, entry.spawnPoint.rotation);
            NetworkObject netObj = obj.GetComponent<NetworkObject>();

            if (netObj != null && !netObj.IsSpawned)
            {
                netObj.Spawn();
            }

            spawnedObjects.Add(obj);
        }

        _enviromentSpawned = true;
    }
}