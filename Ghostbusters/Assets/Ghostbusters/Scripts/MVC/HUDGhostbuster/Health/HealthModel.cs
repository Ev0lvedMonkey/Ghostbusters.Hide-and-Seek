using UnityEngine;

public class HealthModel
{
    public float CurrentHealth { get; private set; }

    public const float MaxHealth = 100f;
    private const float Damage = 5f;

    public HealthModel()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage()
    {
        if (Damage < 0 || Damage > MaxHealth || Damage > CurrentHealth)
        {
            Debug.Log($"ALERT {Damage}");
            return;
        }
        CurrentHealth -= Damage;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

}
