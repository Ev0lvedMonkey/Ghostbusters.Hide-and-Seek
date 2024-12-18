using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSenceSettingUI : MonoBehaviour
{
    [SerializeField] private MouseSenceConfiguration _mouseSenseConfig;
    [SerializeField] private Slider _mouseSenseSlider;
    [SerializeField] private TMP_Text _mouseSenseText;

    public void SetSenseValues()
    {
        _mouseSenseSlider.minValue = _mouseSenseConfig.MinMouseSense;
        _mouseSenseSlider.maxValue = _mouseSenseConfig.MaxMouseSense;
        _mouseSenseSlider.value = _mouseSenseConfig.GetMouseSense();
    }

    public void AddComponentsListeners() =>
        _mouseSenseSlider.onValueChanged.AddListener(OnMouseSenseSliderChanged);

    public void RemoveComponentsListeners() =>
        _mouseSenseSlider.onValueChanged.RemoveListener(OnMouseSenseSliderChanged);

    public void UpdateMouseSenseText() =>
        UpdateMouseSenseText(_mouseSenseSlider.value);

    private void OnMouseSenseSliderChanged(float newSense)
    {
        _mouseSenseConfig.SetMouseSense(newSense);

        UpdateMouseSenseText(_mouseSenseConfig.GetMouseSense());
    }

    private void UpdateMouseSenseText(float sense)
    {
        _mouseSenseText.text = $"{sense:F2}";
    }
}
