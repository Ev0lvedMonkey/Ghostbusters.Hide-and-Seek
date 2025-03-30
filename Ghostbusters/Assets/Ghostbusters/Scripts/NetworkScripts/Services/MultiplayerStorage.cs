using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;

public class MultiplayerStorage : NetworkBehaviour, IService
{
    [SerializeField] private NetworkManager _networkManager;

    public const int MAX_PLAYER_AMOUNT = 4;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";


    internal UnityEvent OnTryingToJoinGame = new();
    internal UnityEvent OnFailedToJoinGame = new();
    internal UnityEvent OnPlayerDataNetworkListChanged = new();

    private NetworkList<PlayerData> _playerDataNetworkList;
    private string _playerName;

    public void Init()
    {
        _playerDataNetworkList = new();

        DontDestroyOnLoad(gameObject);

        //_playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));

        _playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }

    public string GetPlayerName()
    {
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
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        if (_playerDataNetworkList == null)
        {
            Debug.LogError("_playerDataNetworkList is null.");
            return false;
        }

        return playerIndex < _playerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            if (_playerDataNetworkList[i].clientId == clientId)
            {
                Debug.Log($"GetPlayerDataIndexFromClientId Return index {i} from input clientID {clientId}");
                return i;
            }
        }
        return -1;
    }

    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in _playerDataNetworkList)
        {
            if (clientId == 0 && _playerDataNetworkList.Count > 0)
            {
                PlayerData firstPlayerData = _playerDataNetworkList[0];
                return firstPlayerData;
            }
            if (playerData.clientId == clientId)
            {
                Debug.LogError($"GetPlayerDataFromClientId return playerdata {playerData.playerName}");
                Debug.LogError($"GetPlayerDataFromClientId return playerdata {playerData.playerId}");
                Debug.LogError($"GetPlayerDataFromClientId return playerdata {playerData.clientId}");
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
    
    public ulong GetLocalPlayerID()
    {
        return _networkManager.LocalClientId;
    }

    public void KickAllPlayers()
    {
        foreach (var item in _playerDataNetworkList)
        {
            if (NetworkManager.Singleton.IsHost)
                return;
            ServiceLocator.Current.Get<LobbyRelayManager>().KickPlayer(item.playerId.ToString());
            KickPlayer(item.clientId);
        }
    }

    public void KickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        Debug.Log($"DisconnectClient {clientId}");
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
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

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged.Invoke();
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
        Debug.Log($"PlayerDataNetworkList added new player {newPlayer.playerName}\n Player count: {_playerDataNetworkList.Count}");
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
        if (SceneLoader.GetTargetScene().ToString() != SceneLoader.ScenesEnum.CharactersScene.ToString())
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
