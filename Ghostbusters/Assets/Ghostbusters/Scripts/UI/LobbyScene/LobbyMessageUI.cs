using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private GameConnectionHandler _gameConnectionHandler;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    public void Init()
    {
        closeButton.onClick.AddListener(Hide);
        LobbyRelayManager.Instance.OnCreateLobbyStarted.AddListener(LobbyMessage_OnCreateLobbyStarted);
        LobbyRelayManager.Instance.OnCreateLobbyFailed.AddListener(LobbyMessage_OnCreateLobbyFailed);
        LobbyRelayManager.Instance.OnJoinStarted.AddListener(LobbyMessage_OnJoinStarted);
        LobbyRelayManager.Instance.OnJoinFailed.AddListener(LobbyMessage_OnJoinFailed);
        LobbyRelayManager.Instance.OnQuickJoinFailed.AddListener(LobbyMessage_OnQuickJoinFailed);
        MultiplayerStorage.Instance.OnFailedToJoinGame.AddListener(KitchenGameMultiplayer_OnFailedToJoinGame);
        Hide();
    }

    private void LobbyMessage_OnQuickJoinFailed()
    {
        ShowMessage("�� ������� ����� ����� ��� �������� �������������!");
    }

    private void LobbyMessage_OnJoinFailed()
    {
        ShowMessage("�� ������� �������������� � �����!");
    }

    private void LobbyMessage_OnJoinStarted()
    {
        ShowMessage("������������� � �����...");
    }

    private void LobbyMessage_OnCreateLobbyFailed()
    {
        ShowMessage("������ �������� �����. ���������� ��� ���.");
    }

    private void LobbyMessage_OnCreateLobbyStarted()
    {
        ShowMessage("�������� �����...");
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame()
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("�� ������� ������������");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Debug.LogWarning("show " + message);
        Show();
        messageText.text = message;
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