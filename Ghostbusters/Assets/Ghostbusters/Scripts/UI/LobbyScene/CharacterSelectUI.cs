using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _copyCodeButton;
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    private CharacterSelectReady _characterSelectReady;


    public void Init(CharacterSelectReady characterSelectReady)
    {
        _characterSelectReady = characterSelectReady;
        _mainMenuButton.onClick.AddListener(BackToMenu);
        _readyButton.onClick.AddListener(SetReady);
        _copyCodeButton.onClick.AddListener(CopyLobbyCode);
        Show();
    }

    public void Uninit()
    {
        _mainMenuButton.onClick.RemoveListener(BackToMenu);
        _readyButton.onClick.RemoveListener(SetReady);
        _copyCodeButton.onClick.RemoveListener(CopyLobbyCode);
    }

    public void SetLobbyData()
    {
        Lobby lobby = LobbyRelayManager.Instance.GetJoinedLobby();

        _lobbyNameText.text = lobby.Name;
        _lobbyCodeText.text = lobby.LobbyCode;
    }

    private void CopyLobbyCode() =>
        GUIUtility.systemCopyBuffer = _lobbyCodeText.text;

    private void SetReady() =>
        _characterSelectReady.SetPlayerReady();

    private static void BackToMenu()
    {
        NetworkManager.Singleton.Shutdown();
        if (NetworkManager.Singleton.IsHost)
            LobbyRelayManager.Instance.DeleteLobby();
        else
            LobbyRelayManager.Instance.LeaveLobby();
        SceneLoader.Load(SceneLoader.Scene.MenuScene);
    }

    private void Show() =>
        gameObject.SetActive(true);

    private void Hide() =>
        gameObject.SetActive(false);

}
