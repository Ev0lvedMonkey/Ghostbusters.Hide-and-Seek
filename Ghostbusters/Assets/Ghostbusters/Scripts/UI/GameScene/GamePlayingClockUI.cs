using mitaywalle.UICircleSegmentedNamespace;
using UnityEngine;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private UICircleSegmented timerImage;

    private void Update()
    {
        timerImage.fillAmount = GameStateManager.Instance.GetGamePlayingTimerNormalized();
    }
}
