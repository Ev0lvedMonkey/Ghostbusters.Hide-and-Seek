using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ClientPlayerInfo : PlayerInfo
{
    [SerializeField] private Button _kickButton;

    private void Awake()
    {
        _kickButton.onClick.AddListener(KickPlayer);
    }

    private void OnDisable()
    {
        _kickButton.gameObject.SetActive(false);
    }

    private void KickPlayer()
    {
        PlayerData playerData = MultiplayerStorage.Instance.GetPlayerDataFromPlayerIndex(GetPlayerIndex());
        Debug.Log($"pressed kick {MultiplayerStorage.Instance.GetPlayerData().playerId} host: {LobbyRelayManager.Instance.GetJoinedLobby().HostId} k");
        LobbyRelayManager.Instance.KickPlayer(playerData.playerId.ToString());
        MultiplayerStorage.Instance.KickPlayer(playerData.clientId);
    }

    protected override void UpdatePlayerInfo()
    {
        base.UpdatePlayerInfo();
        _kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

    }
}
