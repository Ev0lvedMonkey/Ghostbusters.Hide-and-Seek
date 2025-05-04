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

        LoadLevel(); // Загружаем уровень (без окружения — теперь этим занимается EnvironmentSynchronizer)
        
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

    private void LoadLevel()
    {
        // Уровень, как правило, включает в себя плоскость, стены, UI и т.п. — без динамического окружения
        Instantiate(_gameSceneConfiguration.GetLevelObject());
    }

    private void RegisterServiceLocatorServices()
    {
        ServiceLocator.Current.Register(_firePositionService);
    }
}