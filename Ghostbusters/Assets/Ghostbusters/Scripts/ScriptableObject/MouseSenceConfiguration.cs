using UnityEngine;

[CreateAssetMenu(fileName = "MouseSenceScriptableObject", menuName = "MouseSence")]
public class MouseSenceConfiguration : ScriptableObject
{
    public float MaxMouseSense { get; } = 5f;
    public float MinMouseSense { get; } = 1f;

    [SerializeField] private float _mouseSense;

    private const string MouseSenseKey = "MouseSense";

    private void OnEnable()
    {
        LoadMouseSense();
    }

    private void OnValidate()
    {
        _mouseSense = Mathf.Clamp(_mouseSense, MinMouseSense, MaxMouseSense);
    }

    public void SetMouseSense(float mouseSense)
    {
        _mouseSense = Mathf.Clamp(mouseSense, MinMouseSense, MaxMouseSense);
        CustomPlayerPrefs.SetFloat(MouseSenseKey, _mouseSense);
        PlayerPrefs.Save();
    }

    public float GetMouseSense()
    {
        return _mouseSense;
    }

    private void LoadMouseSense()
    {
        if (CustomPlayerPrefs.HasKey(MouseSenseKey))
        {
            _mouseSense = CustomPlayerPrefs.GetFloat(MouseSenseKey);
            _mouseSense = Mathf.Clamp(_mouseSense, MinMouseSense, MaxMouseSense);
        }
        else
            _mouseSense = Mathf.Clamp(_mouseSense, MinMouseSense, MaxMouseSense);
    }
}
