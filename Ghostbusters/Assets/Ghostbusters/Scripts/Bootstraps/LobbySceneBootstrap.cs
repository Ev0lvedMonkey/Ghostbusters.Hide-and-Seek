using UnityEngine;

public class LobbySceneBootstrap : MonoBehaviour
{
    [SerializeField] private LobbyMessageUI _lobbyMessageUI;
    [SerializeField] private CreateLobbyUI _createLobbyUI;
    [SerializeField] private MainMenuUi _mainMenuUi;
    [SerializeField] private LobbyRelayManager _lobbyRelayManager;
    [SerializeField] private MultiplayerStorage _multiplayerStorage;

    private void Awake()
    {
        _multiplayerStorage.Init();
        _lobbyRelayManager.Init();
    }

    private void OnEnable()
    {
        _lobbyRelayManager.InitializeAuthentication();
        _mainMenuUi.Init();
        _lobbyMessageUI.Init();
        _createLobbyUI.Init();
    }

    private void OnDisable()
    {
        _createLobbyUI.Uninit();
        _lobbyMessageUI.Uninit();
        _mainMenuUi.Uninit();
    }
}
