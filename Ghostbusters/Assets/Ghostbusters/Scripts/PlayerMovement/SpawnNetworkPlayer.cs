using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkPlayer : NetworkBehaviour
{
    private CharactersSpawnPositionList _spawnPositionList;

    private const string PATH = "ScriptableObjects/SpawnPositionListScriptableObject";

    private void Awake()
    {
        CharactersSpawnPositionList spawnPositionList = Resources.Load<CharactersSpawnPositionList>(PATH);
        if (spawnPositionList != null)
            _spawnPositionList = spawnPositionList;        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        ulong clientID = MultiplayerStorage.Instance.GetPlayerData().clientId;
        int playerIndex = MultiplayerStorage.Instance.GetPlayerDataIndexFromClientId(clientID);
        transform.position = _spawnPositionList.GetSpawnPositon(playerIndex);
    }
}
