using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quickButton;

    public void Init()
    {
        _startButton.onClick.AddListener(() => { SceneLoader.Load(SceneLoader.Scene.LobbyScene); });
        _quickButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
