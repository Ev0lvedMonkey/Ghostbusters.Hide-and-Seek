using System;
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
        GameStateManager.Instance.OnOpenHUD.AddListener(() => { gameObject.SetActive(true); });
        GameStateManager.Instance.OnCloseHUD.AddListener(() => { gameObject.SetActive(false); });

        Hide();
    }

    private void GameManager_OnStateChanged()
    {
        switch (GameStateManager.Instance.IsGameOver().ToString())
        {
            case "WinBusters":
                _gameOverWinText.text = "Охотники победили!";
                RemoveListeners();
                Show();
                break;
            case "WinGhost":
                _gameOverWinText.text = "Призраки победи!";
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
        Debug.Log($"{gameObject.name} SHOW");
        _playAgainButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} HIDE");
    }

}
