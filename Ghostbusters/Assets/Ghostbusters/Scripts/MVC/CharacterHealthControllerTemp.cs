using Unity.Netcode;
using UnityEngine;

public class CharacterHealthControllerTemp : NetworkBehaviour
{
    [SerializeField, Range(100, 150)] private float _maxHealth;
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private HealthView _hudView;

    private const float DAMAGE = 5f;

    private float _currentHealth;
    private bool _isDead;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        EnableHUD();
    }

    public void EnableHUD()
    {
        if (IsOwner)
        {
            _hudView.gameObject.SetActive(true);
            Debug.Log($"{gameObject.name} HUD enabled for owner");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            UpdateHUD();
        }
    }

    public void Heal(float healAmount)
    {
        if (_isDead || healAmount <= 0) return;

        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        UpdateHUD();
        UpdateHUDClientRpc(_currentHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log($"{gameObject.name} updated taked damage {_currentHealth}");

        if (_isDead || DAMAGE <= 0) return;

        _currentHealth -= DAMAGE;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            UpdateHUDClientRpc(_currentHealth);  // Обновляем HUD на всех клиентах
        }
    }

    [ClientRpc]
    private void UpdateHUDClientRpc(float newHealth)
    {
        _currentHealth = newHealth;

        if (_hudView != null)
        {
            _hudView.UpdateHealthBar(GetHealthPercentage(), _currentHealth);
        }

        if (IsOwner)
        {
            Debug.Log($"{gameObject.name} updated HUD on owner client with health {_currentHealth}");
        }
    }

    private void UpdateHUD()
    {
        if (_hudView != null)
        {
            _hudView.UpdateHealthBar(GetHealthPercentage(), _currentHealth);
        }
    }

    private void HandleDeath()
    {
        _isDead = true;
        SpawnDeathEffect();

        // Переход в режим наблюдателя
        EnterSpectatorMode();

        // Обновляем HUD на всех клиентах
        UpdateHUDClientRpc(_currentHealth);
    }

    private void SpawnDeathEffect()
    {
        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, transform.position, Quaternion.identity);
        }
    }

    private void EnterSpectatorMode()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }

        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        if (TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.isKinematic = true;
            rigidbody.detectCollisions = false;
        }
        Debug.Log("Entered spectator mode.");
    }

    private float GetHealthPercentage()
    {
        return _currentHealth / _maxHealth;
    }
}
