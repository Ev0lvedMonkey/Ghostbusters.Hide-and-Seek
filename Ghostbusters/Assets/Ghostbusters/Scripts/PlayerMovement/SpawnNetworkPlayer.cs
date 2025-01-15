using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkPlayer : NetworkBehaviour
{
    private CharactersSpawnPositionList _spawnPositionList;
    private ServiceLocator _serviceLocator;

    private const string PATH = "ScriptableObjects/SpawnPositionListScriptableObject";

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        CharactersSpawnPositionList spawnPositionList = Resources.Load<CharactersSpawnPositionList>(PATH);
        if (spawnPositionList != null)
            _spawnPositionList = spawnPositionList;        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        ulong clientID = _serviceLocator.Get<MultiplayerStorage>().GetPlayerData().clientId;
        int playerIndex = _serviceLocator.Get<MultiplayerStorage>().GetPlayerDataIndexFromClientId(clientID);
        transform.position = _spawnPositionList.GetSpawnPositon(playerIndex);
    }
}
