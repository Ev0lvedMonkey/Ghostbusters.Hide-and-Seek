using UnityEngine;

public class GameSceneUIBootstrap : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private GameStartCountdownUI _gameStartCountdownUI;

    [SerializeField] private GamePlayingClockUI _gamePlayingClockUI;
    [SerializeField] private GameOverWinUI _gameOverWinUI;
    [SerializeField] private SettingUI _settingUI;
    [SerializeField] private ChatManager _chatManager;

    public void Init(GameStateManager gameStateManager)
    {
        ServiceLocator.Current.Register(_gameOverWinUI);
        ServiceLocator.Current.Register(_chatManager);
        _gameOverWinUI.Init(gameStateManager);
        _gameStartCountdownUI.Init(gameStateManager);
        _gamePlayingClockUI.Init();
        _settingUI.Init();
        _chatManager.Init();
    }

    public void Uninit()
    {
        _gameOverWinUI.Uninit();
    }
}