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
        kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = MultiplayerStorage.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            LobbyRelayManager.Instance.KickPlayerServerRpc(playerData.playerId.ToString());
            MultiplayerStorage.Instance.KickPlayerServerRpc(playerData.clientId);
        });
    }

    private void Start()
    {
        if (MultiplayerStorage.Instance != null)
        {
            MultiplayerStorage.Instance.OnPlayerDataNetworkListChanged?.AddListener(MultiplayerStorage_OnPlayerDataNetworkListChanged);
            Debug.Log($"MultiplayerStorage add listeners");
        }
        if (CharacterSelectReady.Instance != null)
        {
            CharacterSelectReady.Instance.OnReadyChanged?.AddListener(CharacterSelectReady_OnReadyChanged);
            Debug.Log($"CharacterSelectReady add listeners");
        }
        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

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
        Debug.Log($"{MultiplayerStorage.Instance.IsPlayerIndexConnected(playerIndex)} {playerIndex}");
        if (MultiplayerStorage.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();

            PlayerData playerData = MultiplayerStorage.Instance.GetPlayerDataFromPlayerIndex(playerIndex);

            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));

            playerNameText.text = playerData.playerName.ToString();
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
