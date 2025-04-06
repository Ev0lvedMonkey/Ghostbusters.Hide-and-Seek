using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private const string DynamicTextTable = "DynamicTextTable";
    
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
    }


    private void LobbyMessage_OnQuickJoinFailed()
    {
        ShowMessage(LocalizationSettings.StringDatabase.GetLocalizedString(DynamicTextTable, "QuickJoinFailedText_Key"));
    }

    private void LobbyMessage_OnJoinFailed()
    {
        ShowMessage(LocalizationSettings.StringDatabase.GetLocalizedString(DynamicTextTable, "JoinFailedText_Key"));
    }

    private void LobbyMessage_OnJoinStarted()
    {
        ShowMessage(LocalizationSettings.StringDatabase.GetLocalizedString(DynamicTextTable, "JoinStartedText_Key"));
    }

    private void LobbyMessage_OnCreateLobbyFailed()
    {
        ShowMessage(LocalizationSettings.StringDatabase.GetLocalizedString(DynamicTextTable, "CreateLobbyFailedText_Key"));
    }

    private void LobbyMessage_OnCreateLobbyStarted()
    {
        ShowMessage(LocalizationSettings.StringDatabase.GetLocalizedString(DynamicTextTable, "CreateLobbyStartedText_Key"));
    }
    
    private void ShowMessage(string message)
    {
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