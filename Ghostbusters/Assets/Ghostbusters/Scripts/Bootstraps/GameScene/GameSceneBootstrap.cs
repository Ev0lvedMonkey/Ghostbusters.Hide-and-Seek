using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Service Locator Services")]
    [SerializeField] private FirePositionService _firePositionService;

    [Header("Components")]
    [SerializeField] private GameSceneUIBootstrap _gameSceneUIBootstrap;
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private KeyboardInput _keyboardInput;
    
    [Header("GameScene Config Type")]
    [SerializeField] private GameSceneConfiguration _gameSceneConfiguration;


    private void Awake()
    {
        RegisterServiceLocatorServices();
    }
    
    private void OnEnable()
    {
        ServiceLocator.Current.Register(_gameStateManager);    
        LoadLevelConfig();
        _gameStateManager.Init();
        _gameStateManager.StartCountdown();
        _keyboardInput.Init(_gameStateManager);
        _gameSceneUIBootstrap.Init(_gameStateManager);
    }


    private void OnDisable()
    {
        _gameStateManager.OnStartGame.RemoveAllListeners();
        _gameSceneUIBootstrap.Uninit();
    }

    private void LoadLevelConfig()
    {
        Instantiate(_gameSceneConfiguration.GetLevelObject());
        Instantiate(_gameSceneConfiguration.GetEnviroment());
    }

    private void RegisterServiceLocatorServices()
    {
        ServiceLocator.Current.Register(_firePositionService);
    }
}
