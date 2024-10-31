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

    internal UnityEvent OnTryingToJoinGame = new();
    internal UnityEvent OnFailedToJoinGame = new();
    internal UnityEvent OnPlayerDataNetworkListChanged = new();

    private NetworkList<PlayerData> _playerDataNetworkList;
    private string _playerName;
    public void Init()
    {
        _playerDataNetworkList = new NetworkList<PlayerData>();

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));

        _playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
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
    public void KickPlayerServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_OnClientDisconnectCallback(clientId);

        // Сообщаем клиентам, что игрок был кикнут
        KickPlayerClientRpc(clientId);
    }

    [ClientRpc(RequireOwnership = false)]
    private void KickPlayerClientRpc(ulong clientId)
    {

        Debug.LogError($"ClientRpc");
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            // Загружаем сцену MenuScene только для кикнутого клиента
            SceneLoader.Load(SceneLoader.Scene.MenuScene);
            Debug.LogError($"SceneLoader");
        }

        // Убираем игрока из списка на клиентах
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = _playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                Debug.LogError($"REMOVE {playerData.clientId}");
                _playerDataNetworkList.RemoveAt(i);
            }
        }

        //OnPlayerKicked.Invoke(); // Вызываем событие для обновления UI на клиентах
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.LogError($"START");
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = _playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                Debug.LogError($"REMOVE {playerData.clientId}");
                _playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged.Invoke();
        Debug.LogError($"PLayer data list changed: {_playerDataNetworkList.Count}");

    }

    private void CheckNetowrkManager()
    {
        if (NetworkManager.Singleton != _networkManager)
        {
            Debug.LogWarning($"{gameObject.name} destroind");
            Destroy(_networkManager.gameObject);
            return;
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        PlayerData newPlayer;
        if (Application.isEditor)
        {
            if (clientId == 0)
            {
                int newPlayerClientId = UnityEngine.Random.Range(100, 999);
                newPlayer = new() { clientId = (ulong)newPlayerClientId, playerName = GetPlayerName(), playerId = AuthenticationService.Instance.PlayerId };
            }
            else
                newPlayer = new() { clientId = (ulong)clientId, playerName = GetPlayerName(), playerId = AuthenticationService.Instance.PlayerId };
        }
        else
        {
            newPlayer = new() { clientId = (ulong)clientId, playerName = GetPlayerName(), playerId = AuthenticationService.Instance.PlayerId };
        }
        if (_playerDataNetworkList == null)
        {
            Debug.LogWarning($"{gameObject.name} PlayerDataNetworkList null");
            return;
        }
        _playerDataNetworkList.Add(newPlayer);
        Debug.LogError($"{newPlayer.clientId} NEW PLAYER");
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


    private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke();
    }
}
