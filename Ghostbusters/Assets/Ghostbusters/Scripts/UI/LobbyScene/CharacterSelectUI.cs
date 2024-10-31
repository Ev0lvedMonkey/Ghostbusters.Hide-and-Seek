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

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _mainMenuButton.onClick.AddListener(BackToMenu);
        _readyButton.onClick.AddListener(SetReady);
        _copyCodeButton.onClick.AddListener(CopyLobbyCode);
    }

    private void CopyLobbyCode()
    {
        GUIUtility.systemCopyBuffer = _lobbyCodeText.text;
    }

    private void SetReady()
    {
        CharacterSelectReady.Instance.SetPlayerReady();
    }

    private static void BackToMenu()
    {
        LobbyRelayManager.Instance.LeaveLobby();
        NetworkManager.Singleton.Shutdown();
        SceneLoader.Load(SceneLoader.Scene.MenuScene);
    }

    private void OnEnable()
    {
        Lobby lobby = LobbyRelayManager.Instance.GetJoinedLobby();

        _lobbyNameText.text = lobby.Name;
        _lobbyCodeText.text = lobby.LobbyCode;
    }
}
