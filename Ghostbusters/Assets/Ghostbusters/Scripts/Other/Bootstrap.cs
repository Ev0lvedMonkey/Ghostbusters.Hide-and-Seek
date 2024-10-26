using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LobbyMessageUI _lobbyMessageUI;
    [SerializeField] private MainMenuUi _mainMenuUi;
    [SerializeField] private LobbyRelayManager _lobbyRelayManager;
    [SerializeField] private MultiplayerStorage _multiplayerStorage;


    private void Awake()
    {
        //_lobbyRelayManager.Init();
        //_multiplayerStorage.Init();
        //_lobbyMessageUI.Init();
        //_mainMenuUi.Init();
    }
}
