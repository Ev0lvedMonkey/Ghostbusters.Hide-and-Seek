using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameOverWinUI : MonoBehaviour, IService
{
    public UnityEvent PlayerExit = new();

    [SerializeField] private SettingUI _settingUI;
    [SerializeField] private TextMeshProUGUI _gameOverWinText;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private AudioSource _audioSource;

    private bool _isOpened;
    private bool _winnnerWasDetermined;
    private GameStateManager _gameStateManager;

    public void Init(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
        _playAgainButton.onClick.AddListener(LeaveGame);
        _gameStateManager.OnStateChanged.AddListener(GameManager_OnStateChanged);
        _gameStateManager.OnOpenHUD.AddListener(Show);
        _gameStateManager.OnCloseHUD.AddListener(Hide);
        _settingUI.Init(gameStateManager);
        _settingsButton.onClick.AddListener(()=> _settingUI.Show());
        Hide();
    }

    public void Uninit()
    {
        _playAgainButton.onClick.RemoveListener(LeaveGame);
        _gameStateManager.OnStateChanged.RemoveListener(GameManager_OnStateChanged);
        _gameStateManager.OnOpenHUD.RemoveListener(Show);
        _gameStateManager.OnCloseHUD.RemoveListener(Hide);
    }

    public bool IsOpened()
    {
        return _isOpened;
    }

    private void LeaveGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneLoader.Load(SceneLoader.ScenesEnum.MenuScene);
        ulong clientID = ServiceLocator.Current.Get<MultiplayerStorage>().GetPlayerData().clientId;
        _gameStateManager.ReportPlayerLostServerRpc(clientID);
    }

    private void GameManager_OnStateChanged()
    {
        if (_winnnerWasDetermined)
            return;
        switch (_gameStateManager.GetGameState().ToString())
        {
            case "WinBusters":
                _gameOverWinText.text = LocalizationSettings.StringDatabase.GetLocalizedString("DynamicTextTable", "GhostsbustersWinText_Key");;
                _playAgainButton.gameObject.SetActive(true);
                RemoveListeners();
                Show();
                _winnnerWasDetermined = true;
                _audioSource.Play();
                break;
            case "WinGhost":
                _gameOverWinText.text = LocalizationSettings.StringDatabase.GetLocalizedString("DynamicTextTable", "GhostsWinText_Key");
                _playAgainButton.gameObject.SetActive(true);
                RemoveListeners();
                Show();
                _winnnerWasDetermined = true;
                _audioSource.Play();
                break;
            default:
                break;
        }
    }

    private void RemoveListeners()
    {
        _gameStateManager.OnCloseHUD.RemoveAllListeners();
        _gameStateManager.OnOpenHUD.RemoveAllListeners();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        _isOpened = true;
        CursorController.EnableCursor();
    }

    private void Hide()
    {
        CursorController.DisableCursor();
        _isOpened = false;
        gameObject.SetActive(false);
    }

}
