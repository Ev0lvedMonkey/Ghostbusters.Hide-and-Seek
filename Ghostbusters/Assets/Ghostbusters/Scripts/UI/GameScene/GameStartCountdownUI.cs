using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private int previousCountdownNumber;
    private GameStateManager _gameStateManager;

    public void Init(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
        _gameStateManager.OnStateChanged.AddListener(GameManager_OnStateChanged);
        Show();
    }

    private void GameManager_OnStateChanged()
    {
        if (!_gameStateManager.IsCountdownToStartActive())
        {
            _gameStateManager.OnStartGame.Invoke();
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(_gameStateManager.GetCountdownToStartTimer());
        countdownText.text = $"{countdownNumber}";

        if (previousCountdownNumber != countdownNumber)
            previousCountdownNumber = countdownNumber;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
