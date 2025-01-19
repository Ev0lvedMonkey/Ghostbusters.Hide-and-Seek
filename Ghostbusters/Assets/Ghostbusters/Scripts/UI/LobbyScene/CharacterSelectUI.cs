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
    private ServiceLocator _serviceLocator;


    public void Init(CharacterSelectReady characterSelectReady)
    {
        _characterSelectReady = characterSelectReady;
        _serviceLocator = ServiceLocator.Current;
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
        Lobby lobby = _serviceLocator.Get<LobbyRelayManager>().GetJoinedLobby();

        _lobbyNameText.text = lobby.Name;
        _lobbyCodeText.text = lobby.LobbyCode;
    }

    private void CopyLobbyCode() =>
        GUIUtility.systemCopyBuffer = _lobbyCodeText.text;

    private void SetReady() =>
        _characterSelectReady.SetPlayerReady();

    private void BackToMenu()
    {
        NetworkManager.Singleton.Shutdown();
        if (NetworkManager.Singleton.IsHost)
            _serviceLocator.Get<LobbyRelayManager>().DeleteLobby();
        else
            _serviceLocator.Get<LobbyRelayManager>().LeaveLobby();
        SceneLoader.Load(SceneLoader.ScenesEnum.MenuScene);
    }

    private void Show() =>
        gameObject.SetActive(true);

    private void Hide() =>
        gameObject.SetActive(false);

}
