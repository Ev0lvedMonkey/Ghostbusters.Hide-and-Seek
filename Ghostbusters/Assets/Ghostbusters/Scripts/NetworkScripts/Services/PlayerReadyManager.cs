using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Events;

public class PlayerReadyManager : NetworkBehaviour
{
    private readonly Dictionary<ulong, bool> _readyDict = new();
    private readonly Dictionary<ulong, bool> _localReadyCache = new();

    public UnityEvent OnReadyChanged = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }
    }

    public bool GetReadyStatus(ulong clientId)
    {
        if (IsServer)
        {
            return _readyDict.TryGetValue(clientId, out bool isReady) && isReady;
        }
        else
        {
            return _localReadyCache.TryGetValue(clientId, out bool isReady) && isReady;
        }
    }

    public void SetPlayerReady()
    {
        SubmitReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong sender = rpcParams.Receive.SenderClientId;
        _readyDict[sender] = true;

        BroadcastReadyStatusClientRpc(sender, true);

        if (AllClientsReady())
        {
            ServiceLocator.Current.Get<LobbyRelayManager>().DeleteLobby();
            SceneLoader.LoadNetwork(SceneLoader.ScenesEnum.GameScene);
            CursorController.DisableCursor();
        }
    }

    [ClientRpc]
    private void BroadcastReadyStatusClientRpc(ulong clientId, bool isReady)
    {
        _localReadyCache[clientId] = isReady;
        OnReadyChanged.Invoke();
    }

    private void OnClientConnected(ulong clientId)
    {
        foreach (var kvp in _readyDict)
        {
            SendReadyStatusToNewClientClientRpc(kvp.Key, kvp.Value, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { clientId }
                }
            });
        }
    }

    [ClientRpc]
    private void SendReadyStatusToNewClientClientRpc(ulong otherClientId, bool isReady, ClientRpcParams clientRpcParams = default)
    {
        _localReadyCache[otherClientId] = isReady;
        OnReadyChanged.Invoke();
    }

    private bool AllClientsReady()
    {
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_readyDict.TryGetValue(clientId, out var ready) || !ready)
                return false;
        }
        return true;
    }
}
