using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Events;

public class CharacterSelectReady : NetworkBehaviour
{

    internal UnityEvent OnReadyChanged = new();

    private Dictionary<ulong, bool> playerReadyDictionary;

    public void MakeNewPlayerReadyDictionary()
    {
        playerReadyDictionary = new();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            ServiceLocator.Current.Get<LobbyRelayManager>().DeleteLobby();
            SceneLoader.LoadNetwork(SceneLoader.ScenesEnum.GameScene);
            CursorController.DisableCursor();
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;

        OnReadyChanged.Invoke();
    }


    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }

}