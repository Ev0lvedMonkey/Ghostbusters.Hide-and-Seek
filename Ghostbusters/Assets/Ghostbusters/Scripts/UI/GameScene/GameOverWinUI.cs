using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameOverWinText;
    [SerializeField] private Button _playAgainButton;

    private void Awake()
    {
        _playAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneLoader.Load(SceneLoader.Scene.MenuScene);
        });
    }

    private void Start()
    {
        GameStateManager.Instance.OnStateChanged.AddListener(GameManager_OnStateChanged);

        Hide();
    }

    private void GameManager_OnStateChanged()
    {
        switch (GameStateManager.Instance.IsGameOver().ToString())
        {
            case "WinBusters":
                _gameOverWinText.text = "Busters WIN!";
                Show();
                break;
            case "WinGhost":
                _gameOverWinText.text = "GHOST WIN!";
                Show();
                break;
            default:
                _gameOverWinText.text = "Something wrong";
                break;
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Debug.Log($"{gameObject.name} SHOW");
        _playAgainButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} HIDE");
    }

}
