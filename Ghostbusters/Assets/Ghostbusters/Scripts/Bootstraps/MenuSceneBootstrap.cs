using UnityEngine;

public class MenuSceneBootstrap : MonoBehaviour
{
    [SerializeField] private MouseSenceSettingUI _mouseSenceSettingUI;
    [SerializeField] private ScreenModeChanger _screenModeChanger;
    [SerializeField] private SingltonsCleanUp _singltonCleanUp;
    [SerializeField] private CamRotateAround _camRotateAround;
    [SerializeField] private MenuUI _menuUI;
    [SerializeField] private SettingUI _settingUI;
    [SerializeField] private HelpUI _helpUI;
    [SerializeField] private AudioManager _audioManager;

    private void Awake()
    {
        _singltonCleanUp.CleanUp();
        ServiceLocator.Inizialize();
        ServiceLocator.Current.Register(_audioManager);
        _audioManager.Init();
        _camRotateAround.DefinitionOfRotateDirection();
        _camRotateAround.InitHeight();
        CursorController.EnableCursor();
        _settingUI.Init();
        _helpUI.Init();
        _menuUI.Init();
        _mouseSenceSettingUI.SetSenseValues();
        _mouseSenceSettingUI.UpdateMouseSenseText();
        _mouseSenceSettingUI.AddComponentsListeners();
        _screenModeChanger.DetermineScreenMode();
        _screenModeChanger.AddComponentsListeners();
    }

    private void OnDisable()
    {
        _screenModeChanger.RemoveComponentsListeners();
        _mouseSenceSettingUI.RemoveComponentsListeners();
    }
}
