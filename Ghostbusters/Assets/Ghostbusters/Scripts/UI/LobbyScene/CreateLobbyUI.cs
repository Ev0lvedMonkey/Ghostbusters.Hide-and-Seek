using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    public void Init()
    {
        lobbyNameInputField.characterLimit = 14;
        createPublicButton.onClick.AddListener(() => CreateLobby(false));
        createPrivateButton.onClick.AddListener(() => CreateLobby(true));
        closeButton.onClick.AddListener(() => Hide());
        Hide();
    }

    public void Uninit()
    {
        createPublicButton.onClick.RemoveListener(() => CreateLobby(false));
        createPrivateButton.onClick.RemoveListener(() => CreateLobby(true));
        closeButton.onClick.RemoveListener(() => Hide());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private async void CreateLobby(bool isPrivate)
    {
        await ServiceLocator.Current.Get<LobbyRelayManager>().CreateLobby(lobbyNameInputField.text, isPrivate);
    }
}