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
        GameStateManager.Instance.OnStateChanged.AddListener(KitchenGameManager_OnStateChanged);

        Hide();
    }

    private void KitchenGameManager_OnStateChanged()
    {
        Show();
        switch (GameStateManager.Instance.IsGameOver().ToString())
        {
            case "WinBusters":
                _gameOverWinText.text = "Busters WIN!";
                break;
            case "WinGhost":
                _gameOverWinText.text = "GHOST WIN!";
                break;
            default:
                _gameOverWinText.text = "Something wrong";
                break;

        }


    }

    private void Show()
    {
        gameObject.SetActive(true);
        _playAgainButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
