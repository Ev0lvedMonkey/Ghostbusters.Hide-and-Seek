using UnityEngine;
using UnityEngine.UI;

public class ScreenModeChanger : MonoBehaviour
{
    private const string FullscreenKey = "FullscreenMode";

    [SerializeField] private Toggle fullscreenToggle; 
    [SerializeField] private Toggle1 toggleController; 


    public void DetermineScreenMode()
    {
        bool isFullscreen = PlayerPrefs.GetInt(FullscreenKey, 0) == 1;
        SetScreenMode(isFullscreen);
    }

    public void AddComponentsListeners() =>

        fullscreenToggle.onValueChanged.AddListener(OnToggleChanged);

    public void RemoveComponentsListeners() =>

        fullscreenToggle.onValueChanged.RemoveListener(OnToggleChanged);

    private void OnToggleChanged(bool isFullscreen)
    {
        SetScreenMode(isFullscreen);

        PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetScreenMode(bool isFullscreen)
    {
        toggleController.OnPointerDown(!isFullscreen);
        fullscreenToggle.isOn = isFullscreen;
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
