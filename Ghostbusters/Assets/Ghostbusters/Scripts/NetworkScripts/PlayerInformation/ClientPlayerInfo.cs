using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ClientPlayerInfo : PlayerInfo
{
    [SerializeField] private Button _kickButton;

    private void OnDisable()
    {
        _kickButton.gameObject.SetActive(false);
    }

    public override void Init(CharacterSelectReady characterSelectReady)
    {
        base.Init(characterSelectReady);
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
