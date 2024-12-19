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

    private void Awake()
    {
        RegisterServiceLocatorServices();
    }

    private void OnEnable()
    {
        _gameStateManager.Init();
        _gameOverWinUI.Init();
        _gameStateManager.StartCountdown();
        _gameStartCountdownUI.Init();
        _gamePlayingClockUI.Init();
        _gamePlayingClockUI.Hide();
    }


    private void OnDisable()
    {
        _gameStateManager.OnStartGame.RemoveAllListeners();
        _gameOverWinUI.Uninit();
    }

    private void RegisterServiceLocatorServices()
    {
        ServiceLocator.Inizialize();
        ServiceLocator.Current.Register(_firePositionService);
    }
}
