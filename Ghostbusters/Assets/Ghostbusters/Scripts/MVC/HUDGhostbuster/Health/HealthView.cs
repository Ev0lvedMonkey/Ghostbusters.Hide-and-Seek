using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void UpdateHealthBar(float healthPercentage, float currentHealth)
    {
        _healthBarImage.fillAmount = healthPercentage;
        _healthText.text = currentHealth.ToString();
    }
}
