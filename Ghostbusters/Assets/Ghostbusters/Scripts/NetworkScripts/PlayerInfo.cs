using TMPro;
using UnityEngine;

public abstract class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyGameObject;
    [SerializeField] private TextMeshProUGUI _playerNameText;


    private void Start()
    {
        if (MultiplayerStorage.Instance != null)
        {
            MultiplayerStorage.Instance.OnPlayerDataNetworkListChanged.AddListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        }
        if (CharacterSelectReady.Instance != null)
        {
            CharacterSelectReady.Instance.OnReadyChanged.AddListener(CharacterSelectReady_OnReadyChanged);
        }
        UpdatePlayer();
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
                _readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(0));
            else
                _readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
        }
        else
            _readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
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
