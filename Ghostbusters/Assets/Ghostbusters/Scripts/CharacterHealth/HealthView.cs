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
        _healthBarImage.fillAmount = healthPercentage;
        _healthText.text = currentHealth.ToString();
        Debug.Log($"Obj {gameObject.name} View update");
    }
}
