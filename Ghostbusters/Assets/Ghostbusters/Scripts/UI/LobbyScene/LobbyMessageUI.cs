using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;
    private ServiceLocator _serviceLocator;

    public void Init()
    {
        _serviceLocator = ServiceLocator.Current;
        closeButton.onClick.AddListener(Hide);
        _serviceLocator.Get<LobbyRelayManager>().OnCreateLobbyStarted.AddListener(LobbyMessage_OnCreateLobbyStarted);
        _serviceLocator.Get<LobbyRelayManager>().OnCreateLobbyFailed.AddListener(LobbyMessage_OnCreateLobbyFailed);
        _serviceLocator.Get<LobbyRelayManager>().OnJoinStarted.AddListener(LobbyMessage_OnJoinStarted);
        _serviceLocator.Get<LobbyRelayManager>().OnJoinFailed.AddListener(LobbyMessage_OnJoinFailed);
        _serviceLocator.Get<LobbyRelayManager>().OnQuickJoinFailed.AddListener(LobbyMessage_OnQuickJoinFailed);
        _serviceLocator.Get<MultiplayerStorage>().OnFailedToJoinGame.AddListener(KitchenGameMultiplayer_OnFailedToJoinGame);
        Hide();
    }

    public void Uninit()
    {
        closeButton.onClick.RemoveListener(Hide);
        _serviceLocator.Get<LobbyRelayManager>().OnCreateLobbyStarted.RemoveListener(LobbyMessage_OnCreateLobbyStarted);
        _serviceLocator.Get<LobbyRelayManager>().OnCreateLobbyFailed.RemoveListener(LobbyMessage_OnCreateLobbyFailed);
        _serviceLocator.Get<LobbyRelayManager>().OnJoinStarted.RemoveListener(LobbyMessage_OnJoinStarted);
        _serviceLocator.Get<LobbyRelayManager>().OnJoinFailed.RemoveListener(LobbyMessage_OnJoinFailed);
        _serviceLocator.Get<LobbyRelayManager>().OnQuickJoinFailed.RemoveListener(LobbyMessage_OnQuickJoinFailed);
        _serviceLocator.Get<MultiplayerStorage>().OnFailedToJoinGame.RemoveListener(KitchenGameMultiplayer_OnFailedToJoinGame);
    }


    private void LobbyMessage_OnQuickJoinFailed()
    {
        ShowMessage("Не удалось найти лобби для быстрого присоединения!");
    }

    private void LobbyMessage_OnJoinFailed()
    {
        ShowMessage("Не удалось присоединиться к лобби!");
    }

    private void LobbyMessage_OnJoinStarted()
    {
        ShowMessage("Присоединение к лобби...");
    }

    private void LobbyMessage_OnCreateLobbyFailed()
    {
        ShowMessage("Ошибка создания лобби. Попробуйте ещё раз.");
    }

    private void LobbyMessage_OnCreateLobbyStarted()
    {
        ShowMessage("Создание лобби...");
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame()
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Не удалось подключиться");
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