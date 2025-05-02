using TMPro;
using UnityEngine;

public abstract class PlayerInfo : MonoBehaviour
{
    [Header("Base Сomponents")]
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyGameObject;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    protected ServiceLocator _serviceLocator;
    protected MultiplayerStorage _multiplayerStorage;
    private CharacterSelectReady _characterSelectReady;

    public virtual void Init(CharacterSelectReady characterSelectReady)
    {
        _characterSelectReady = characterSelectReady;
        _serviceLocator = ServiceLocator.Current;
        _multiplayerStorage = _serviceLocator.Get<MultiplayerStorage>(); 
        
        _multiplayerStorage.OnPlayerDataNetworkListChanged.AddListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
        _characterSelectReady.OnReadyChanged.AddListener(CharacterSelectReady_OnReadyChanged);

        UpdatePlayer();
    }

    public virtual void Uninit()
    {
        _multiplayerStorage.OnPlayerDataNetworkListChanged.RemoveListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
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
        if (_multiplayerStorage.IsPlayerIndexConnected(_playerIndex))
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

        PlayerData playerData = _multiplayerStorage.GetPlayerDataFromPlayerIndex(_playerIndex);

        bool isReady = _characterSelectReady.GetReadyStatus(playerData.clientId);

        _readyGameObject.SetActive(isReady);

        UpdatePlayerName(playerData);
    }

    private void UpdatePlayerName(PlayerData playerData)
    {
        ulong localClientId = _multiplayerStorage.GetPlayerData().clientId;

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
