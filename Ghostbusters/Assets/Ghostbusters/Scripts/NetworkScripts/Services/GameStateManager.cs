using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public enum State
{
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    WinBusters,
    WinGhost
}

public class GameStateManager : NetworkBehaviour, IService
{
    [Header("Match Duration Configuration")]
    [SerializeField] private MatchDurationConfiguration _matchDurationConfig;

    [Header("Character`s prefabs")]
    [SerializeField] private Transform ghostPrefab;
    [SerializeField] private Transform playerPrefab;

    private NetworkList<bool> _playerStatusList = new();
    private NetworkVariable<State> _state = new(State.WaitingToStart);
    private bool _isLocalPlayerReady;
    private NetworkVariable<float> _startDelay;
    private NetworkVariable<float> _gamePlayingTimer = new(0f);
    private float _gameDuration;
    private ServiceLocator _serviceLocator;

    internal UnityEvent OnStateChanged = new();
    internal UnityEvent OnLocalPlayerReadyChanged = new();
    internal UnityEvent OnStartGame = new();
    internal UnityEvent OnCloseHUD = new();
    internal UnityEvent OnOpenHUD = new();
    internal UnityEvent OnSecretRoomOpen = new();
    internal UnityEvent OnChatOpen = new();
    internal UnityEvent OnChatClose = new();

    private void Update()
    {
        GameStateHandler();
    }

    public void Init()
    {
        _serviceLocator = ServiceLocator.Current;
        _startDelay = new NetworkVariable<float>(_matchDurationConfig.StartDelay);
        _gameDuration = _matchDurationConfig.GameDuration;
    }

    public override void OnNetworkSpawn()
    {
        _state.OnValueChanged += State_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    public void StartCountdown()
    {
        _state.Value = State.CountdownToStart;
        Debug.Log($"CountdownToStart started");
    }

    public bool IsCountdownToStartActive()
    {
        return _state.Value == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _startDelay.Value;
    }

    public State GetGameState()
    {
        return _state.Value;
    }

    public bool IsLocalPlayerReady()
    {
        return _isLocalPlayerReady;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer.Value / _gameDuration);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReportPlayerLostServerRpc(ulong clientId)
    {
        int playerIndex = _serviceLocator.Get<PlayerSessionManager>().GetPlayerDataIndexFromClientId(clientId);
        Debug.Log($"ReportPlayerLostServerRpc invoke player with clientID {clientId}, his index {playerIndex}");
        if (playerIndex != -1) _playerStatusList[playerIndex] = true;
        int index = 0;
        foreach (bool item in _playerStatusList)
        {
            Debug.Log($"layerStatusList index {index} is dead? - {item}");
            index++;
        }

        CheckWinCondition();
    }

    private void GameStateHandler()
    {
        if (!IsServer)
        {
            return;
        }

        switch (_state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _startDelay.Value -= Time.deltaTime;
                if (_startDelay.Value < 0f)
                {
                    _state.Value = State.GamePlaying;
                    _gamePlayingTimer.Value = _gameDuration;
                }

                break;
            case State.GamePlaying:
                _gamePlayingTimer.Value -= Time.deltaTime;
                if (_gamePlayingTimer.Value <= _gameDuration / 2)
                    OnSecretRoomOpen.Invoke();
                if (_gamePlayingTimer.Value < 0f)
                {
                    _state.Value = State.WinGhost;
                }

                break;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName,
        UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted,
        List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform;
            if (IsGhost(clientId))
            {
                playerTransform = Instantiate(ghostPrefab);
            }
            else
            {
                playerTransform = Instantiate(playerPrefab);
            }

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            _playerStatusList.Add(false);
        }
    }

    private bool IsGhost(ulong clientId)
    {
        int idPlayer =
            _serviceLocator.Get<PlayerSessionManager>().GetPlayerDataIndexFromClientId(_serviceLocator
                .Get<PlayerSessionManager>().GetPlayerDataFromClientId(clientId).clientId);
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

        if (allGhostsLost) _state.Value = State.WinBusters;
        else if (allBustersLost) _state.Value = State.WinGhost;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke();
        Debug.Log($"State changed from {previousValue} to {newValue}");
    }
}