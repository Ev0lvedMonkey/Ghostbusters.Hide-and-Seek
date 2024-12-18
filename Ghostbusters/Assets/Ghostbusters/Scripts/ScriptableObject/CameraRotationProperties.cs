using UnityEngine;

[CreateAssetMenu(fileName = "CameraRotationPropertiesScriptableObject", menuName = "CameraRotationProperties")]
public class CameraRotationProperties : ScriptableObject
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float waveAmplitude;
    [SerializeField] private float waveFrequency;

    public float RotationSpeed => rotationSpeed;
    public float WaveAmplitude => waveAmplitude;
    public float WaveFrequency => waveFrequency;
}