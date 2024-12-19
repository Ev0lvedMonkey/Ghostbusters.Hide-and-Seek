using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private int previousCountdownNumber;


    public void Init()
    {
        GameStateManager.Instance.OnStateChanged.AddListener(GameManager_OnStateChanged);
        Show();
    }

    private void GameManager_OnStateChanged()
    {
        if (!GameStateManager.Instance.IsCountdownToStartActive())
        {
            GameStateManager.Instance.OnStartGame.Invoke();
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameStateManager.Instance.GetCountdownToStartTimer());
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
