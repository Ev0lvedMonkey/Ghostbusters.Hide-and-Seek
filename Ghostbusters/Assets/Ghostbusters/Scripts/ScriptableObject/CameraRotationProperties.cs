using UnityEngine;

[CreateAssetMenu(
    fileName = "CameraRotationPropertiesScriptableObject",
    menuName = "CameraRotationProperties")]
public class CameraRotationProperties : ScriptableObject
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _waveAmplitude;
    [SerializeField] private float _waveFrequency;

    public float RotationSpeed => _rotationSpeed;
    public float WaveAmplitude => _waveAmplitude;
    public float WaveFrequency => _waveFrequency;
}