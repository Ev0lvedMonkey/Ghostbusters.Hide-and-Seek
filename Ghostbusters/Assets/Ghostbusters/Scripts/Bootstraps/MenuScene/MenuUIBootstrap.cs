using UnityEngine;

public class MenuUIBootstrap : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private SettingUI _settingsUI;
    [SerializeField] private MenuUI _menuUI;
    [SerializeField] private ModernHelpUI _helpUI;
    
    public void Init()
    {
        _settingsUI.Init();
        _helpUI.Init();
        _menuUI.Init();
    }

    public void Uninit()
    {
        _settingsUI.Uninit();
    }
}