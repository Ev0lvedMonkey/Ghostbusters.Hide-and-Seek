using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        GameStateManager.Instance.OnLocalPlayerReadyChanged.AddListener(KitchenGameManager_OnLocalPlayerReadyChanged);
        GameStateManager.Instance.OnStateChanged.AddListener(KitchenGameManager_OnStateChanged);

        Hide();
    }

    private void KitchenGameManager_OnStateChanged()
    {
        if (GameStateManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged()
    {
        if (GameStateManager.Instance.IsLocalPlayerReady())
        {
            Show();
        }
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
