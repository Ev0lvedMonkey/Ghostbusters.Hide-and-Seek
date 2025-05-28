using UnityEngine;
using UnityEngine.Serialization;

public class LobbySceneBootstrap : MonoBehaviour
{
    [SerializeField] private LobbyUIBootstrap _lobbyUIBootstrap;
    
    [Header("Net Managers")]
    [SerializeField] private LobbyRelayManager _lobbyRelayManager;
    [FormerlySerializedAs("_multiplayerStorage")] [SerializeField] private PlayerSessionManager playerSessionManager;

    private void Awake()
    {
        ServiceLocator.Current.Register(_lobbyRelayManager);
        ServiceLocator.Current.Register(playerSessionManager);
        
        playerSessionManager.Init();
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
