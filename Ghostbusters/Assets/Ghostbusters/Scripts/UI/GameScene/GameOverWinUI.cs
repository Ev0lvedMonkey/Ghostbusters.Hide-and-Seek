using System.Globalization;
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


    private void Awake()
    {
        Instance = this;
        _playAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneLoader.Load(SceneLoader.Scene.MenuScene);
            try
            {
                ulong clientID = MultiplayerStorage.Instance.GetPlayerData().clientId;
                GameStateManager.Instance.ReportPlayerLostServerRpc(clientID);

            }
            catch { }
            PlayerExit.Invoke();
        });
    }

    private void Start()
    {
        GameStateManager.Instance.OnStateChanged.AddListener(GameManager_OnStateChanged);
        GameStateManager.Instance.OnOpenHUD.AddListener(() => { Show(); });
        GameStateManager.Instance.OnCloseHUD.AddListener(() => { Hide(); });
        //_playAgainButton.gameObject.SetActive(false);
        Hide();
    }

    private void GameManager_OnStateChanged()
    {
        switch (GameStateManager.Instance.IsGameOver().ToString())
        {
            case "WinBusters":
                _gameOverWinText.text = "Охотники победили!";
                _playAgainButton.gameObject.SetActive(true);
                RemoveListeners();
                Show();
                break;
            case "WinGhost":
                _gameOverWinText.text = "Призраки победили!";
                _playAgainButton.gameObject.SetActive(true);
                RemoveListeners();
                Show();
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
        CursorController.EnableCursor();
    }

    private void Hide()
    {
        CursorController.DisableCursor();
        gameObject.SetActive(false);
    }

}
