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

    private void OnEnable()
    {
        _kickButton.gameObject.SetActive(true);
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
    }

    public override void Uninit()
    {
        base.Uninit();
        _kickButton.onClick.RemoveListener(KickPlayer);
    }
    protected override void UpdatePlayerInfo()
    {
        base.UpdatePlayerInfo();
        _kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
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