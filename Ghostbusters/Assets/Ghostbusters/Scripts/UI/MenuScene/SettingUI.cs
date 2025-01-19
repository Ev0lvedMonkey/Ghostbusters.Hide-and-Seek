using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [Header("Components")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TMP_Text _musicText;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private TMP_Text _effectsText;
    [SerializeField] private Button _exitButton;

    [Header("Config")]
    [SerializeField] private AudioVolumeConfiguration _audioConfig;

    private GameStateManager _gameStateManager;

    public void Init(GameStateManager gameStateManager = null)
    {
        _gameStateManager = gameStateManager;
        _gameStateManager?.OnCloseHUD.AddListener(Hide);
        _exitButton.onClick.AddListener(Hide);

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
        Hide();
    }

    public void Show() =>
        gameObject.SetActive(true);

    public void Hide() =>
        gameObject.SetActive(false);

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
        if (value <= 0.01f)
            _mixerGroup.audioMixer.SetFloat(_audioConfig.MusicKey, -80f);
        else
            _mixerGroup.audioMixer.SetFloat(_audioConfig.MusicKey, Mathf.Log10(value) * 20);
    }

    private void SetEffectsVolume(float value)
    {
        if (value <= 0.01f)
            _mixerGroup.audioMixer.SetFloat(_audioConfig.EffectsKey, -80f);
        else
            _mixerGroup.audioMixer.SetFloat(_audioConfig.EffectsKey, Mathf.Log10(value) * 20);
    }

    private void UpdateMusicText(float value)
    {
        _musicText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void UpdateEffectsText(float value)
    {
        _effectsText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
