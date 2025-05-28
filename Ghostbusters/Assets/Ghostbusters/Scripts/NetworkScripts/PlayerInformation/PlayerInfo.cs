using TMPro;
using UnityEngine;

public abstract class PlayerInfo : MonoBehaviour
{
    [Header("Base Components")]
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyGameObject;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    protected ServiceLocator _serviceLocator;
    protected PlayerSessionManager _playerSessionManager;
    private PlayerReadyManager _playerReadyManager;

    public virtual void Init(PlayerReadyManager playerReadyManager)
    {
        _playerReadyManager = playerReadyManager;
        _serviceLocator = ServiceLocator.Current;
        _playerSessionManager = _serviceLocator.Get<PlayerSessionManager>(); 
        
        _playerSessionManager.OnPlayerDataNetworkListChanged.AddListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        _playerReadyManager.OnReadyChanged.AddListener(CharacterSelectReady_OnReadyChanged);

        UpdatePlayer();
    }

    public virtual void Uninit()
    {
        _playerSessionManager.OnPlayerDataNetworkListChanged.RemoveListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        _playerReadyManager?.OnReadyChanged.RemoveListener(CharacterSelectReady_OnReadyChanged);
        _playerReadyManager = null;
    }

    protected int GetPlayerIndex()
    {
        return _playerIndex;
    }

    protected virtual void UpdatePlayerInfo()
    {
        Show();

        PlayerData playerData = _playerSessionManager.GetPlayerDataFromPlayerIndex(_playerIndex);

        bool isReady = _playerReadyManager.GetReadyStatus(playerData.clientId);

        _readyGameObject.SetActive(isReady);

        UpdatePlayerName(playerData);
        Debug.Log($"[Playerinfo] Update player index {_playerIndex}, name {_playerNameText.text} ");
    }

    private void CharacterSelectReady_OnReadyChanged()
    {
        UpdatePlayer();
    }

    private void MultiplayerStorage_OnPlayerDataNetworkListChanged()
    {
        UpdatePlayer();
    }

    protected void UpdatePlayer()
    {
        if (_playerSessionManager.IsPlayerIndexConnected(_playerIndex))
        {
            UpdatePlayerInfo();
        }
        else
        {
            Hide();
        }
    }


    private void UpdatePlayerName(PlayerData playerData)
    {
        ulong localClientId = _playerSessionManager.GetPlayerData().clientId;

        _playerNameText.color = playerData.clientId == localClientId ? Color.green : Color.white;
        _playerNameText.text = playerData.playerName.ToString();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
