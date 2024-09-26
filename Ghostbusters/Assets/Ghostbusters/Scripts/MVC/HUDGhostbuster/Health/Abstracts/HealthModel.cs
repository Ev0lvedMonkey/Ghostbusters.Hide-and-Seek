using UnityEngine;

public abstract class HealthModel
{   
    public float CurrentHealth { get; private set; }

    private const float Damage = 5f;

    public float _maxHealth;

    public HealthModel(float maxHealth)
    {
        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;
    }

    public virtual void TakeDamage()
    {
        if (Damage < 0 || Damage > _maxHealth || Damage > CurrentHealth)
        {
            Debug.Log($"ALERT {Damage}");
            return;
        }
        CurrentHealth -= Damage;

    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / _maxHealth;
    }

}
