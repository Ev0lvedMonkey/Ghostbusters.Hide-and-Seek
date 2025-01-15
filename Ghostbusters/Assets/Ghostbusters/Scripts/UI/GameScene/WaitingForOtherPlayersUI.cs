using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private GameStateManager _gameStateManager;
    private void Start()
    {
        _gameStateManager = ServiceLocator.Current.Get<GameStateManager>();
        _gameStateManager.OnLocalPlayerReadyChanged.AddListener(KitchenGameManager_OnLocalPlayerReadyChanged);
        _gameStateManager.OnStateChanged.AddListener(KitchenGameManager_OnStateChanged);

        Hide();
    }

    private void KitchenGameManager_OnStateChanged()
    {
        if (_gameStateManager.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged()
    {
        if (_gameStateManager.IsLocalPlayerReady())
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
