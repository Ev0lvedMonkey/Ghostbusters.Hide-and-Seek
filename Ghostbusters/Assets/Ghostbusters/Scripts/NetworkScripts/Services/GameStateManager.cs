using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : NetworkBehaviour, IService
{
    internal UnityEvent OnStateChanged = new();
    internal UnityEvent OnLocalGamePaused = new();
    internal UnityEvent OnLocalGameUnpaused = new();
    internal UnityEvent OnMultiplayerGamePaused = new();
    internal UnityEvent OnMultiplayerGameUnpaused = new();
    internal UnityEvent OnLocalPlayerReadyChanged = new();
    internal UnityEvent OnStartGame = new();
    internal UnityEvent OnCloseHUD = new();
    internal UnityEvent OnOpenHUD = new();
    internal UnityEvent OnSecretRoomOpen = new();
    internal UnityEvent OnChatOpen = new();
    internal UnityEvent OnChatClose = new();


    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        WinBusters,
        WinGhost
    }

    [SerializeField] private Transform ghostPrefab;
    [SerializeField] private Transform playerPrefab;

    private NetworkList<bool> _playerStatusList = new();
    private NetworkVariable<State> state = new(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> countdownToStartTimer = new(10f);
    private NetworkVariable<float> gamePlayingTimer = new(0f);
    private float gamePlayingTimerMax = 60f;
    private bool isLocalGamePaused = false;
    private NetworkVariable<bool> isGamePaused = new(false);
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;
    private bool autoTestGamePausedState;
    private ServiceLocator _serviceLocator;


    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value <= gamePlayingTimerMax / 2)
                    OnSecretRoomOpen.Invoke();
                if (gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.WinGhost;
                }
                break;

        }
    }

    private void LateUpdate()
    {
        if (autoTestGamePausedState)
        {
            autoTestGamePausedState = false;
            TestGamePausedState();
        }
    }

    public void Init()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    public void StartCountdown()
    {
        state.Value = State.CountdownToStart;
        Debug.LogWarning($"CountdownToStart started");
    }


    public void InvokeStartGameEvent() =>
        OnStartGame.Invoke();

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }

    public State GetGameState()
    {
        return state.Value;
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer.Value / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isLocalGamePaused = !isLocalGamePaused;
        if (isLocalGamePaused)
        {
            PauseGameServerRpc();

            OnLocalGamePaused.Invoke();
        }
        else
        {
            UnpauseGameServerRpc();

            OnLocalGameUnpaused.Invoke();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReportPlayerLostServerRpc(ulong clientId)
    {
        int playerIndex = _serviceLocator.Get<MultiplayerStorage>().GetPlayerDataIndexFromClientId(clientId);
        Debug.Log($"ReportPlayerLostServerRpc invoke player with clientID {clientId}, his index {playerIndex}");
        if (playerIndex != -1) _playerStatusList[playerIndex] = true;
        int index = 0;
        foreach (var item in _playerStatusList)
        {
            Debug.Log($"layerStatusList index {index} is dead? - {item}");
            index++;
        }

        CheckWinCondition();
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform;
            if (IsGhost(clientId))
            {
                playerTransform = Instantiate(ghostPrefab);
                Debug.Log("GHOST Instantiated");
            }
            else
            {
                playerTransform = Instantiate(playerPrefab);
                Debug.Log("buster Instantiated");
            }
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            Debug.Log($"SceneManager_OnLoadEventCompleted added new player on status list with clientID {clientId}");
            _playerStatusList.Add(false);
        }
    }

    private bool IsGhost(ulong clientId)
    {
        int idPlayer =
            _serviceLocator.Get<MultiplayerStorage>().GetPlayerDataIndexFromClientId(_serviceLocator.Get<MultiplayerStorage>().GetPlayerDataFromClientId(clientId).clientId);
        if (idPlayer == 0 || idPlayer == 2) return true;
        else return false;
    }

    private void CheckWinCondition()
    {
        bool allGhostsLost = false;
        bool allBustersLost = false;
        if (_playerStatusList.Count <= 2)
        {
            for (int i = 0; i < _playerStatusList.Count; i++)
            {
                if (_playerStatusList[0] == true)
                    allGhostsLost = true;
                else if (_playerStatusList[1] == true)
                    allBustersLost = true;
                else continue;
            }
        }
        else if (_playerStatusList.Count == 3)
        {
            for (int i = 0; i < _playerStatusList.Count; i++)
            {
                if (_playerStatusList[0] == true && _playerStatusList[2] == true)
                    allGhostsLost = true;
                else if (_playerStatusList[1] == true)
                    allBustersLost = true;
                else continue;

            }
        }
        else if (_playerStatusList.Count == 4)
        {
            for (int i = 0; i < _playerStatusList.Count; i++)
            {
                if (_playerStatusList[0] == true && _playerStatusList[2] == true)
                    allGhostsLost = true;
                else if (_playerStatusList[1] == true && _playerStatusList[3] == true)
                    allBustersLost = true;
                else continue;

            }
        }
        else
        {
            allGhostsLost = false;
            allBustersLost = false;
        }
        if (allGhostsLost) state.Value = State.WinBusters;
        else if (allBustersLost) state.Value = State.WinGhost;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        autoTestGamePausedState = true;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;

            OnMultiplayerGamePaused.Invoke();
        }
        else
        {
            Time.timeScale = 1f;

            OnMultiplayerGameUnpaused.Invoke();
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke();
        Debug.Log($"State changed from {previousValue} to {newValue}");
    }

    [ServerRpc(RequireOwnership = false)]

    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();

        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            state.Value = State.CountdownToStart;
            Debug.Log($"CountdownToStart started");
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePausedState();
    }

    private void TestGamePausedState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                isGamePaused.Value = true;
                return;
            }
        }

        isGamePaused.Value = false;
    }
}
