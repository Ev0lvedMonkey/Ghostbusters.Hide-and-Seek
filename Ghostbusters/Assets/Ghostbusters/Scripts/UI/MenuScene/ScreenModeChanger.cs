using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScreenModeChanger : MonoBehaviour
{
    [SerializeField] private Toggle _fullscreenToggle; 
    [SerializeField] private CustomToggle _toggleController; 

    private const string FullscreenKey = "FullscreenMode";

    public void DetermineScreenMode()
    {
        bool isFullscreen = CustomPlayerPrefs.GetInt(FullscreenKey, 0) == 1;
        SetScreenMode(isFullscreen);
    }

    public void AddComponentsListeners() =>

        _fullscreenToggle.onValueChanged.AddListener(OnToggleChanged);

    public void RemoveComponentsListeners() =>

        _fullscreenToggle.onValueChanged.RemoveListener(OnToggleChanged);

    private void OnToggleChanged(bool isFullscreen)
    {
        SetScreenMode(isFullscreen);

        CustomPlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetScreenMode(bool isFullscreen)
    {
        _toggleController.OnPointerDown(!isFullscreen);
        _fullscreenToggle.isOn = isFullscreen;
        if (isFullscreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
            Screen.fullScreen = isFullscreen;
        }
        else
        {
            Screen.fullScreen = isFullscreen;
        }
    }
}
