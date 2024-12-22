using System;
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
        LoadLevelConfig();
        _gameStateManager.Init();
        _gameOverWinUI.Init();
        _gameStateManager.StartCountdown();
        _gameStartCountdownUI.Init();
        _gamePlayingClockUI.Init();
        _gamePlayingClockUI.Hide();
        _mouseSenceSettingUI.SetSenseValues();
        _mouseSenceSettingUI.UpdateMouseSenseText();
        _mouseSenceSettingUI.AddComponentsListeners();
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
        Instantiate(_gameSceneConfiguration.GetEnviroment());
    }

    private void RegisterServiceLocatorServices()
    {
        ServiceLocator.Inizialize();
        ServiceLocator.Current.Register(_firePositionService);
    }
}
