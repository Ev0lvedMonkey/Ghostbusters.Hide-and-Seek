using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class MultiplayerStorage : NetworkBehaviour
{
    [SerializeField] private NetworkManager _networkManager;

    public const int MAX_PLAYER_AMOUNT = 4;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public static MultiplayerStorage Instance { get; private set; }

    internal UnityEvent OnTryingToJoinGame;
    internal UnityEvent OnFailedToJoinGame;
    internal UnityEvent OnPlayerDataNetworkListChanged;

    private NetworkList<PlayerData> _playerDataNetworkList;
    private string _playerName;

    private void Awake()
    {
        _playerDataNetworkList = new NetworkList<PlayerData>();

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.LogError($"{gameObject.name} singlton instanced");

        _playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));

        _playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
        if (_playerDataNetworkList != null)
        {
            Debug.Log($"_playerDataNetworkList COOOOOOL");
        }
    }


    public string GetPlayerName()
    {
        Debug.Log($"{gameObject.name}: get player name {_playerName}");
        return _playerName;
    }

    public void SetPlayerName(string playerName)
    {
        this._playerName = playerName;

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
        Debug.Log($"{gameObject.name}: set player name {playerName}");
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke();

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
        Debug.LogWarning($"{gameObject.name}: start client");
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = null;

        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
        CheckNetowrkManager();
        Debug.LogWarning($"{gameObject.name}: start host");
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        Debug.LogWarning($"PlayerDataNetworkList.Count {_playerDataNetworkList.Count}");
        return playerIndex < _playerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            if (_playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }

    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in _playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                return playerData;
            }
        }
        return default;
    }

    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return _playerDataNetworkList[playerIndex];
    }

    [ServerRpc(RequireOwnership = false)]
    public void KickPlayerServerRpc(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
    }
    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke();
    }

    private void CheckNetowrkManager()
    {
        if (NetworkManager.Singleton != _networkManager)
        {
            Destroy(_networkManager.gameObject);
            Debug.LogError($"{gameObject.name} destroind");
            return;
        }
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = _playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                _playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        PlayerData newPlayer;
        Debug.LogWarning($"Client wanna add ID: {clientId}");
        if (clientId == 0)
        {
            int newPlayerClientId = UnityEngine.Random.Range(100, 999);
            newPlayer = new() { clientId = (ulong)newPlayerClientId };
        }
        else
            newPlayer = new() { clientId = clientId };
        Debug.LogWarning($"AND : {newPlayer.clientId}");
        if (_playerDataNetworkList == null)
        {
            Debug.LogError($"{gameObject.name} PlayerDataNetworkList null");
            return;
        }
        _playerDataNetworkList.Add(newPlayer);
        Debug.LogWarning($"Player added ID: {clientId}");
        Debug.LogWarning($"_playerDataNetworkList count: {_playerDataNetworkList.Count}");
        string playerName = GetPlayerName();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name is null or empty.");
            return;
        }
        string playerId = AuthenticationService.Instance.PlayerId;
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogWarning("PlayerId is null or empty.");
            return;
        }
        SetPlayerNameServerRpc(playerName);
        SetPlayerIdServerRpc(playerId);
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneLoader.GetTargetScene().ToString() != SceneLoader.Scene.CharactersScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        connectionApprovalResponse.Approved = true;
    }

    private void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = _playerDataNetworkList[playerDataIndex];

        playerData.playerName = playerName;

        _playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = _playerDataNetworkList[playerDataIndex];

        playerData.playerId = playerId;

        _playerDataNetworkList[playerDataIndex] = playerData;
    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke();
    }
}
