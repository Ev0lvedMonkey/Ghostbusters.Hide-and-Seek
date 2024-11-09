using UnityEngine;

[CreateAssetMenu(fileName = "MouseSenceScriptableObject", menuName = "MouseSence", order = 51)]
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
        PlayerPrefs.SetFloat(MouseSenseKey, _mouseSense);
        PlayerPrefs.Save();
    }

    public float GetMouseSense()
    {
        return _mouseSense;
    }

    private void LoadMouseSense()
    {
        if (PlayerPrefs.HasKey(MouseSenseKey))
        {
            _mouseSense = PlayerPrefs.GetFloat(MouseSenseKey);
            _mouseSense = Mathf.Clamp(_mouseSense, MinMouseSense, MaxMouseSense);
        }
        else
            _mouseSense = Mathf.Clamp(_mouseSense, MinMouseSense, MaxMouseSense);
    }
}
