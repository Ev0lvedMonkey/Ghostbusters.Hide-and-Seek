using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void UpdateHealthBar(float healthPercentage, float currentHealth)
    {
        Debug.Log($"{gameObject.name} get new HealthView {healthPercentage} {currentHealth}");
        _healthBarImage.fillAmount = healthPercentage;
        _healthText.text = currentHealth.ToString();
    }
}
