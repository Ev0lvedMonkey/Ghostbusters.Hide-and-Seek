using UnityEngine;

[CreateAssetMenu(fileName = "AudioVolumeConfiguration", menuName = "AudioVolumeScriptableObject")]
public class AudioVolumeConfiguration : ScriptableObject
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _musicVolume = 0.75f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _effectsVolume = 0.75f;

    public readonly string MusicKey = "MusicValue";
    public readonly string EffectsKey = "EffectsValue";

    private void OnEnable()
    {
        LoadVolumes();
    }

    public float MusicVolume => _musicVolume;
    public float EffectsVolume => _effectsVolume;

    public void SetMusicVolume(float value)
    {
        _musicVolume = Mathf.Clamp01(value);
        CustomPlayerPrefs.SetFloat(MusicKey, _musicVolume);
        PlayerPrefs.Save();
    }

    public void SetEffectsVolume(float value)
    {
        _effectsVolume = Mathf.Clamp01(value);
        CustomPlayerPrefs.SetFloat(EffectsKey, _effectsVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolumes()
    {
        _musicVolume = CustomPlayerPrefs.GetFloat(MusicKey, 0.75f);
        _effectsVolume = CustomPlayerPrefs.GetFloat(EffectsKey, 0.75f);
    }
}
