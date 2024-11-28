using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    private void Awake()
    {
        createPublicButton.onClick.AddListener(() =>
        {
            CreateLobby(false);
        });
        createPrivateButton.onClick.AddListener(() =>
        {
            CreateLobby(true);
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }

    private async void CreateLobby(bool isPrivate) =>
                 await LobbyRelayManager.Instance.CreateLobby(lobbyNameInputField.text, isPrivate);

    public void Show()
    {
        gameObject.SetActive(true);

        createPublicButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
