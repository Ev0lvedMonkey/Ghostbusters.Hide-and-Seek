using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ClientPlayerInfo : PlayerInfo
{
    [SerializeField] private Button _kickButton;
    
    private int _playerIndex;
    
    private void OnDisable()
    {
        _kickButton.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        KickPlayer();
    }

    public override void Init(PlayerReadyManager playerReadyManager)
    {
        base.Init(playerReadyManager);
        _playerIndex = GetPlayerIndex();
        _kickButton.onClick.AddListener(KickPlayer);
        _kickButton.onClick.AddListener(() => _kickButton.gameObject.SetActive(false));
    }

    public override void Uninit()
    {
        base.Uninit();
        _kickButton.onClick.RemoveListener(KickPlayer);
        _kickButton.onClick.RemoveListener(() => _kickButton.gameObject.SetActive(false));
    }
    protected override void UpdatePlayerInfo()
    {
        base.UpdatePlayerInfo();
        _kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        Debug.Log($"[ClientPlayerInfo] PlayerIndex: {_playerIndex} KickButton: {} ");
    }

    private void KickPlayer()
    {
        PlayerData playerData = _playerSessionManager.GetPlayerDataFromPlayerIndex(_playerIndex);
        if (_playerSessionManager.IsPlayerIndexConnected(_playerIndex))
        {
            Debug.Log($"Will kick player {playerData.playerName} playerIndex {_playerIndex}");
            _serviceLocator.Get<LobbyRelayManager>().KickPlayer(playerData.playerId.ToString());
            _playerSessionManager.KickPlayer(playerData.clientId);
        }

        UpdatePlayer();
    }
}