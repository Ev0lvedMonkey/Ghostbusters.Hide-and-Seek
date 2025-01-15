using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverWinUI : MonoBehaviour, IService
{
    public UnityEvent PlayerExit = new();

    [SerializeField] private TextMeshProUGUI _gameOverWinText;
    [SerializeField] private Button _playAgainButton;

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
        SceneLoader.Load(SceneLoader.Scene.MenuScene);
        ulong clientID = ServiceLocator.Current.Get<MultiplayerStorage>().GetPlayerData().clientId;
        _gameStateManager.ReportPlayerLostServerRpc(clientID);
        PlayerExit.Invoke();
    }

    private void GameManager_OnStateChanged()
    {
        if (_winnnerWasDetermined)
            return;
        switch (_gameStateManager.GetGameState().ToString())
        {
            case "WinBusters":
                _gameOverWinText.text = "Охотники победили!";
                _playAgainButton.gameObject.SetActive(true);
                RemoveListeners();
                Show();
                _winnnerWasDetermined = true;
                break;
            case "WinGhost":
                _gameOverWinText.text = "Призраки победили!";
                _playAgainButton.gameObject.SetActive(true);
                RemoveListeners();
                Show();
                _winnnerWasDetermined = true;
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
