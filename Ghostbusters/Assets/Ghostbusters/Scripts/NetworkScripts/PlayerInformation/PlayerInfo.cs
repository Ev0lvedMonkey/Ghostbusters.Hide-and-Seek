using TMPro;
using UnityEngine;

public abstract class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyGameObject;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    protected ServiceLocator _serviceLocator;
    private CharacterSelectReady _characterSelectReady;

    public virtual void Init(CharacterSelectReady characterSelectReady)
    {
        _characterSelectReady = characterSelectReady;
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Get<MultiplayerStorage>().OnPlayerDataNetworkListChanged.AddListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        _characterSelectReady.OnReadyChanged.AddListener(CharacterSelectReady_OnReadyChanged);
        UpdatePlayer();
    }

    public virtual void Uninit()
    {
        ServiceLocator.Current.Get<MultiplayerStorage>().OnPlayerDataNetworkListChanged.RemoveListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        _characterSelectReady?.OnReadyChanged.RemoveListener(CharacterSelectReady_OnReadyChanged);
        _characterSelectReady = null;
    }

    private void CharacterSelectReady_OnReadyChanged()
    {
        UpdatePlayer();
    }


    private void MultiplayerStorage_OnPlayerDataNetworkListChanged()
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (_serviceLocator.Get<MultiplayerStorage>().IsPlayerIndexConnected(_playerIndex))
        {
            UpdatePlayerInfo();
        }
        else
        {
            Hide();
        }
    }

    protected int GetPlayerIndex()
    {
        return _playerIndex;
    }

    protected virtual void UpdatePlayerInfo()
    {
        Show();

        PlayerData playerData = _serviceLocator.Get<MultiplayerStorage>().GetPlayerDataFromPlayerIndex(_playerIndex);
        if (Application.isEditor)
        {
            if (_serviceLocator.Get<LobbyRelayManager>().IsLobbyHost())
                _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(0));
            else
                _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(playerData.clientId));
        }
        else
            _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(playerData.clientId));
        ulong clientID = _serviceLocator.Get<MultiplayerStorage>().GetPlayerData().clientId;

        if (playerData.clientId == clientID)
            _playerNameText.color = Color.green;
        else
            _playerNameText.color = Color.white;
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
