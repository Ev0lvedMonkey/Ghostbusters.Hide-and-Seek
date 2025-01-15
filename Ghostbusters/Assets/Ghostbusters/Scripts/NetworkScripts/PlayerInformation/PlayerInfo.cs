using TMPro;
using UnityEngine;

public abstract class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyGameObject;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    private CharacterSelectReady _characterSelectReady;

    public virtual void Init(CharacterSelectReady characterSelectReady)
    {
        _characterSelectReady = characterSelectReady;
        if (MultiplayerStorage.Instance != null)
        {
            MultiplayerStorage.Instance.OnPlayerDataNetworkListChanged.AddListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        }
        _characterSelectReady?.OnReadyChanged.AddListener(CharacterSelectReady_OnReadyChanged);
        UpdatePlayer();
    }

    public virtual void Uninit()
    {
        if (MultiplayerStorage.Instance != null)
        {
            MultiplayerStorage.Instance.OnPlayerDataNetworkListChanged.RemoveListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        }
        _characterSelectReady.OnReadyChanged.RemoveListener(CharacterSelectReady_OnReadyChanged);
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
        if (MultiplayerStorage.Instance.IsPlayerIndexConnected(_playerIndex))
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

        PlayerData playerData = MultiplayerStorage.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
        if (Application.isEditor)
        {
            if (LobbyRelayManager.Instance.IsLobbyHost())
                _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(0));
            else
                _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(playerData.clientId));
        }
        else
            _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(playerData.clientId));
        ulong clientID = MultiplayerStorage.Instance.GetPlayerData().clientId;

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
