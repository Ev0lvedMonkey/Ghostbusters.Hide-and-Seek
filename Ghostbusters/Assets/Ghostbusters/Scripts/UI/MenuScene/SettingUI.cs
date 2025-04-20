using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class SettingUI : MonoBehaviour
{
    private enum Language
    {
        ru,
        en
    }

    [Header("Mixer")] 
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [Header("Components")]
    [SerializeField] private Slider _musicSlider;

    [SerializeField] private TMP_Text _musicText;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private TMP_Text _effectsText;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _languageButton;
    [SerializeField] private ScreenModeChanger _screenModeChanger;
    [SerializeField] private MouseSenceSettingUI _mouseSenceSettingUI;

    [Header("Config")] [SerializeField] private AudioVolumeConfiguration _audioConfig;

    private GameStateManager _gameStateManager;
    private Language _selectedLanguage;
    private const string LanguageKey = "SelectedLanguage";

    public void Init(GameStateManager gameStateManager = null)
    {
        _gameStateManager = gameStateManager;
        _gameStateManager?.OnCloseHUD.AddListener(Hide);
        _exitButton.onClick.AddListener(Hide);
        _languageButton.onClick.AddListener(ToggleLanguage);

        float savedMusicValue = _audioConfig.MusicVolume;
        float savedEffectsValue = _audioConfig.EffectsVolume;

        _musicSlider.value = savedMusicValue;
        _effectsSlider.value = savedEffectsValue;

        SetMusicVolume(savedMusicValue);
        SetEffectsVolume(savedEffectsValue);
        UpdateMusicText(savedMusicValue);
        UpdateEffectsText(savedEffectsValue);

        _musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        _effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);

        LoadLanguage();
        Hide();
        InitSubSettings();
    }

    public void Uninit()
    {
        _musicSlider.onValueChanged.RemoveAllListeners();
        _effectsSlider.onValueChanged.RemoveAllListeners();
        _screenModeChanger.RemoveComponentsListeners();
        _mouseSenceSettingUI.RemoveComponentsListeners();
    }


    private void InitSubSettings()
    {
        _mouseSenceSettingUI.SetSenseValues();
        _mouseSenceSettingUI.UpdateMouseSenseText();
        _mouseSenceSettingUI.AddComponentsListeners();
        _screenModeChanger.DetermineScreenMode();
        _screenModeChanger.AddComponentsListeners();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnMusicSliderChanged(float value)
    {
        SetMusicVolume(value);
        UpdateMusicText(value);
        _audioConfig.SetMusicVolume(value);
    }

    private void OnEffectsSliderChanged(float value)
    {
        SetEffectsVolume(value);
        UpdateEffectsText(value);
        _audioConfig.SetEffectsVolume(value);
    }

    private void SetMusicVolume(float value)
    {
        _mixerGroup.audioMixer.SetFloat(_audioConfig.MusicKey, value <= 0.01f ? -80f : Mathf.Log10(value) * 20);
    }

    private void SetEffectsVolume(float value)
    {
        _mixerGroup.audioMixer.SetFloat(_audioConfig.EffectsKey, value <= 0.01f ? -80f : Mathf.Log10(value) * 20);
    }

    private void UpdateMusicText(float value)
    {
        _musicText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void UpdateEffectsText(float value)
    {
        _effectsText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void ToggleLanguage()
    {
        _selectedLanguage = _selectedLanguage == Language.ru ? Language.en : Language.ru;
        ChangeLocale(_selectedLanguage);
        CustomPlayerPrefs.SetInt(LanguageKey, (int)_selectedLanguage);
    }

    private void ChangeLocale(Language localeCode)
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode.ToString());
        if (locale == null)
        {
            return;
        }
        LocalizationSettings.SelectedLocale = locale;
        Debug.Log($"Localization changed to {locale}");
    }

    private void LoadLanguage()
    {
        _selectedLanguage = (Language)CustomPlayerPrefs.GetInt(LanguageKey, (int)Language.ru);
        ChangeLocale(_selectedLanguage);
    }
}