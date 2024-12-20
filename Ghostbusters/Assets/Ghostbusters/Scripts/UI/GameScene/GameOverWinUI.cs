using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverWinUI : MonoBehaviour
{
    public static GameOverWinUI Instance;

    public UnityEvent PlayerExit = new();

    [SerializeField] private TextMeshProUGUI _gameOverWinText;
    [SerializeField] private Button _playAgainButton;

    private bool _isOpened;
    private bool _winnnerWasDetermined;

    public void Init()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        _playAgainButton.onClick.AddListener(LeaveGame);
        GameStateManager.Instance.OnStateChanged.AddListener(GameManager_OnStateChanged);
        GameStateManager.Instance.OnOpenHUD.AddListener(Show);
        GameStateManager.Instance.OnCloseHUD.AddListener(Hide);
        Hide();
    }

    public void Uninit()
    {
        _playAgainButton.onClick.RemoveListener(LeaveGame);
        GameStateManager.Instance.OnStateChanged.RemoveListener(GameManager_OnStateChanged);
        GameStateManager.Instance.OnOpenHUD.RemoveListener(Show);
        GameStateManager.Instance.OnCloseHUD.RemoveListener(Hide);
    }

    public bool IsOpened()
    {
        return _isOpened;
    }

    private void LeaveGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneLoader.Load(SceneLoader.Scene.MenuScene);
        ulong clientID = MultiplayerStorage.Instance.GetPlayerData().clientId;
        GameStateManager.Instance.ReportPlayerLostServerRpc(clientID);
        PlayerExit.Invoke();
    }

    private void GameManager_OnStateChanged()
    {
        if (_winnnerWasDetermined)
            return;
        switch (GameStateManager.Instance.GetGameState().ToString())
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
        GameStateManager.Instance.OnCloseHUD.RemoveAllListeners();
        GameStateManager.Instance.OnOpenHUD.RemoveAllListeners();
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
