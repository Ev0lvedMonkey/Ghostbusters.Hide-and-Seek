using UnityEngine;

[CreateAssetMenu(fileName = "MatchDurationConfigurationScriptableObject",
    menuName = "MatchDurationConfiguration")]
public class MatchDurationConfiguration : ScriptableObject
{
    private const float MinGameDuration = 30f;
    private const float MaxGameDuration = 120f;
    private const float MinStartDelay = 5f;
    private const float MaxStartDelay = 30f;
    
    [SerializeField, Range(MinGameDuration, MaxGameDuration)] private float _gameDuration;
    [SerializeField, Range(MinStartDelay, MaxStartDelay)] private float _startDelay;
    
    public float GameDuration => _gameDuration;
    public float StartDelay => _startDelay;
}