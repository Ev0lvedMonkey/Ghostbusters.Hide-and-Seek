using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Service Locator Services")]
    [SerializeField] private FirePositionService _firePositionService;

    [Header("Components")]
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private GameStartCountdownUI _gameStartCountdownUI;
    [SerializeField] private GamePlayingClockUI _gamePlayingClockUI;
    [SerializeField] private GameOverWinUI _gameOverWinUI;
    [SerializeField] private MouseSenceSettingUI _mouseSenceSettingUI;
    [SerializeField] private KeyboardInput _keyboardInput;
    [SerializeField] private ChatManager _chatManager;

    [Header("GameScene Config Type")]
    [SerializeField] private int _numberOfConfigType;

    private GameSceneConfiguration _gameSceneConfiguration;

    private const string LoadPath = "ScriptableObjects/GameSceneConfig";

    private void Awake()
    {
        RegisterServiceLocatorServices();
    }


    private void OnEnable()
    {
        ServiceLocator.Current.Register(_gameStateManager);    
        ServiceLocator.Current.Register(_gameOverWinUI);
        ServiceLocator.Current.Register(_chatManager);
        LoadLevelConfig();
        _gameStateManager.Init();
        _gameOverWinUI.Init(_gameStateManager);
        _gameStateManager.StartCountdown();
        _gameStartCountdownUI.Init(_gameStateManager);
        _gamePlayingClockUI.Init();
        _gamePlayingClockUI.Hide();
        _mouseSenceSettingUI.SetSenseValues();
        _mouseSenceSettingUI.UpdateMouseSenseText();
        _mouseSenceSettingUI.AddComponentsListeners();
        _keyboardInput.Init(_gameStateManager);
        _chatManager.Init();
    }


    private void OnDisable()
    {
        _gameStateManager.OnStartGame.RemoveAllListeners();
        _gameOverWinUI.Uninit();
        _mouseSenceSettingUI.RemoveComponentsListeners();
    }

    private void LoadLevelConfig()
    {
        _gameSceneConfiguration = Resources.Load<GameSceneConfiguration>(LoadPath + _numberOfConfigType);
        Instantiate(_gameSceneConfiguration.GetLevelObject());
        Instantiate(_gameSceneConfiguration.GetTerrainObject());
        Instantiate(_gameSceneConfiguration.GetEnviroment());
    }

    private void RegisterServiceLocatorServices()
    {
        ServiceLocator.Current.Register(_firePositionService);
    }
}
