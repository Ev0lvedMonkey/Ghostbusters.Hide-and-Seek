using mitaywalle.UICircleSegmentedNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private UICircleSegmented timerImage;

    private void Update()
    {
        timerImage.fillAmount = GameStateManager.Instance.GetGamePlayingTimerNormalized();
    }
}
