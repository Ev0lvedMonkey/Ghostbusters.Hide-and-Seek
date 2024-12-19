using mitaywalle.UICircleSegmentedNamespace;
using UnityEngine;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private UICircleSegmented timerImage;


    private void Update()
    {
        timerImage.fillAmount = GameStateManager.Instance.GetGamePlayingTimerNormalized();
    }
    
    public void Init()
    {
        GameStateManager.Instance.OnStartGame.AddListener(Show);
    }

    public void Show()=>
        gameObject.SetActive(true);
    public void Hide()=>
        gameObject.SetActive(false);
}
