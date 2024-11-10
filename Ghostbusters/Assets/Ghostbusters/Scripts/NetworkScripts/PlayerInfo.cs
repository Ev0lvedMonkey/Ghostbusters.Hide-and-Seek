using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private Button kickButton;
    [SerializeField] private TextMeshProUGUI playerNameText;


    private void Awake()
    {
        kickButton.onClick.AddListener(KickPlayer);
    }


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


    private void CheckKickButton(PlayerData playerData)
    {
            kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
    }

    private void CharacterSelectReady_OnReadyChanged()
    {
        UpdatePlayer();
    }

    private void KickPlayer()
    {
        PlayerData playerData = MultiplayerStorage.Instance.GetPlayerData();
        Debug.Log($"pressed kick {playerData.playerId} {LobbyRelayManager.Instance.GetJoinedLobby().HostId}");
        LobbyRelayManager.Instance.KickPlayer(playerData.playerId.ToString());
        MultiplayerStorage.Instance.KickPlayerServerRpc(playerData.clientId);
    }


    private void MultiplayerStorage_OnPlayerDataNetworkListChanged()
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (MultiplayerStorage.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();

            PlayerData playerData = MultiplayerStorage.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            if (Application.isEditor)
            {
                if (LobbyRelayManager.Instance.IsLobbyHost())
                    readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(0));
                else
                    readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            }
            else
                readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            playerNameText.text = playerData.playerName.ToString();
            //CheckKickButton(playerData);
        }
        else
        {
            Hide();
        }
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
