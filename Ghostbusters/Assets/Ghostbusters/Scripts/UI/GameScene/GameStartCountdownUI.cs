using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private int previousCountdownNumber;


    private void Start()
    {
        GameStateManager.Instance.OnStateChanged.AddListener(KitchenGameManager_OnStateChanged);
        Hide();
    }

    private void KitchenGameManager_OnStateChanged()
    {
        if (GameStateManager.Instance.IsCountdownToStartActive())
            Show();
        else
            Hide();
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameStateManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

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
