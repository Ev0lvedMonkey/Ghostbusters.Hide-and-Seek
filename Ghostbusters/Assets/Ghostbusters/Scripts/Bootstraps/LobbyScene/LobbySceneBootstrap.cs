using UnityEngine;

public class LobbySceneBootstrap : MonoBehaviour
{
    [SerializeField] private LobbyUIBootstrap _lobbyUIBootstrap;
    
    [Header("Net Managers")]
    [SerializeField] private LobbyRelayManager _lobbyRelayManager;
    [SerializeField] private MultiplayerStorage _multiplayerStorage;

    private void Awake()
    {
        ServiceLocator.Current.Register(_lobbyRelayManager);
        ServiceLocator.Current.Register(_multiplayerStorage);
        
        _multiplayerStorage.Init();
        _lobbyRelayManager.Init();
    }

    private void OnEnable()
    {
        _lobbyRelayManager.InitializeAuthentication();
        _lobbyUIBootstrap.Init();
    }

    private void OnDisable()
    {
        _lobbyUIBootstrap.Uninit();
    }
}
