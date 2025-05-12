using UnityEngine;

[CreateAssetMenu(fileName = "AudioVolumeConfiguration", menuName = "AudioVolumeScriptableObject")]
public class AudioVolumeConfiguration : ScriptableObject
{
    public readonly string MusicKey = "MusicValue";
    public readonly string EffectsKey = "EffectsValue";

    [SerializeField, Range(0.0f, 1.0f)] private float _musicVolume = 0.75f;
    [SerializeField, Range(0.0f, 1.0f)] private float _effectsVolume = 0.75f;

    public float MusicVolume => _musicVolume;
    public float EffectsVolume => _effectsVolume;

    private void OnEnable()
    {
        LoadVolumes();
    }

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
