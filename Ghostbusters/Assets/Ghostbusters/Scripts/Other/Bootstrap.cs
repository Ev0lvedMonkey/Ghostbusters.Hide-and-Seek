using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LobbyMessageUI _lobbyMessageUI;
    [SerializeField] private MainMenuUi _mainMenuUi;
    [SerializeField] private LobbyRelayManager _lobbyRelayManager;
    [SerializeField] private MultiplayerStorage _multiplayerStorage;

    private void Awake()
    {
        _multiplayerStorage.Init();
        _lobbyRelayManager.Init();
        _mainMenuUi.Init();
        _lobbyRelayManager.InitializeAuthentication();
        _lobbyMessageUI.Init();
    }
}
