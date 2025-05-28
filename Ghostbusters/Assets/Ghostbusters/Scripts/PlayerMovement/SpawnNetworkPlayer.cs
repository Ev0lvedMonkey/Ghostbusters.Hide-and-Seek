using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private CharactersSpawnPositionList _spawnPositionList;

    private ServiceLocator _serviceLocator;
    
    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        ulong clientID = _serviceLocator.Get<PlayerSessionManager>().GetPlayerData().clientId;
        int playerIndex = _serviceLocator.Get<PlayerSessionManager>().GetPlayerDataIndexFromClientId(clientID);
        transform.position = _spawnPositionList.GetSpawnPositon(playerIndex);
    }
}
