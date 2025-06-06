using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _quickButton;

    [Header("UI Components")]
    [SerializeField] private SettingUI _settingUI;
    [SerializeField] private ModernHelpUI _helpUI;

    public void Init()
    {
        _startButton.onClick.AddListener(() =>  SceneLoader.Load(SceneLoader.ScenesEnum.LobbyScene));
        _settingsButton.onClick.AddListener(() => _settingUI.Show());
        _helpButton.onClick.AddListener(() => _helpUI.Show());
        _quickButton.onClick.AddListener(() => Application.Quit());
    }
}
