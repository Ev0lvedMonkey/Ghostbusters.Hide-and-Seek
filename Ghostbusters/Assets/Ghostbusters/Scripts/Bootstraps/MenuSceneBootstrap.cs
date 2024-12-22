using UnityEngine;

public class MenuSceneBootstrap : MonoBehaviour
{
    [SerializeField] private MouseSenceSettingUI _mouseSenceSettingUI;
    [SerializeField] private ScreenModeChanger _screenModeChanger;
    [SerializeField] private SingltonsCleanUp _singltonCleanUp;
    [SerializeField] private CamRotateAround _camRotateAround;
    [SerializeField] private MenuUI _menuUI;


    private void Awake()
    {
        _singltonCleanUp.CleanUp();
        _camRotateAround.DefinitionOfRotateDirection();
        _camRotateAround.InitHeight();
        CursorController.EnableCursor();
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
