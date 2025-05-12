using mitaywalle.UICircleSegmentedNamespace;
using UnityEngine;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private UICircleSegmented timerImage;

    private GameStateManager _gameStateManager;

    private void Update()
    {
        timerImage.fillAmount = _gameStateManager.GetGamePlayingTimerNormalized();
    }

    public void Init()
    {
        _gameStateManager = ServiceLocator.Current.Get<GameStateManager>();
        _gameStateManager.OnStartGame.AddListener(Show);
        Hide();
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