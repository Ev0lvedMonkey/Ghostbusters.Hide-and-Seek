using System;
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

    public override void Init(CharacterSelectReady characterSelectReady)
    {
        base.Init(characterSelectReady);
        _playerIndex = GetPlayerIndex();
        _kickButton.onClick.AddListener(KickPlayer);
        _kickButton.onClick.AddListener(() => _kickButton.gameObject.SetActive(false));
    }

    public override void Uninit()
    {
        base.Uninit();
        _kickButton.onClick.RemoveListener(KickPlayer);
        _kickButton.onClick.RemoveListener(() => _kickButton.gameObject.SetActive(false));
        _kickButton.gameObject.SetActive(false);
    }

    private void KickPlayer()
    {
        PlayerData playerData = _multiplayerStorage.GetPlayerDataFromPlayerIndex(_playerIndex);
        
        Debug.Log($"pressed kick {_multiplayerStorage.GetPlayerData().playerId}" +
                  $" host: {_serviceLocator.Get<LobbyRelayManager>().GetJoinedLobby().HostId} k");
        
        _serviceLocator.Get<LobbyRelayManager>().KickPlayer(playerData.playerId.ToString());
        _multiplayerStorage.KickPlayer(playerData.clientId);
    }

    protected override void UpdatePlayerInfo()
    {
        base.UpdatePlayerInfo();
        _kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
    }
}
