using UnityEngine;

public class LobbyUIBootstrap : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private LobbyMessageUI _lobbyMessageUI;
    [SerializeField] private CreateLobbyUI _createLobbyUI;
    [SerializeField] private MainMenuUi _mainMenuUi;

    public void Init()
    {
        _mainMenuUi.Init();
        _lobbyMessageUI.Init();
        _createLobbyUI.Init();
    }

    public void Uninit()
    {
        _createLobbyUI.Uninit();
        _lobbyMessageUI.Uninit();
        _mainMenuUi.Uninit();
    }
}