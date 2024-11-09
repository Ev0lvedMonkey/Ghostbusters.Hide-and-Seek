using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSenceSettingUI : MonoBehaviour
{
    [SerializeField] private MouseSenceConfiguration _mouseSenseConfig;
    [SerializeField] private Slider _mouseSenseSlider;
    [SerializeField] private TMP_Text _mouseSenseText;

    private void OnEnable()
    {
        _mouseSenseSlider.minValue = _mouseSenseConfig.MinMouseSense;
        _mouseSenseSlider.maxValue = _mouseSenseConfig.MaxMouseSense;
        _mouseSenseSlider.value = _mouseSenseConfig.GetMouseSense();
        UpdateMouseSenseText(_mouseSenseSlider.value);

        _mouseSenseSlider.onValueChanged.AddListener(OnMouseSenseSliderChanged);
    }

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
