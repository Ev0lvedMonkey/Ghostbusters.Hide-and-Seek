using mitaywalle.UICircleSegmentedNamespace;
using UnityEngine;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private UICircleSegmented timerImage;


    private void Update()
    {
        timerImage.fillAmount = ServiceLocator.Current.Get<GameStateManager>().GetGamePlayingTimerNormalized();
    }
    
    public void Init()
    {
        ServiceLocator.Current.Get<GameStateManager>().OnStartGame.AddListener(Show);
    }

    public void Show()=>
        gameObject.SetActive(true);
    public void Hide()=>
        gameObject.SetActive(false);
}
