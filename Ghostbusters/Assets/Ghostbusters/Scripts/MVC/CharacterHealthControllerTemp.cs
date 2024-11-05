using Unity.Netcode;
using UnityEngine;

public class CharacterHealthControllerTemp : NetworkBehaviour
{
    [SerializeField, Range(100, 150)] private float _maxHealth;
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Rigidbody _bodyRigidbody;
    [SerializeField] private HealthView _hudView;

    private const int SELF_DAMAGE = 50;
    private const int GHOST_DAMAGE = 50;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        if (IsOwner)
            EnableHUD();
    }

    public void EnableHUD()
    {
        _hudView.gameObject.SetActive(true);
        Debug.Log($"{gameObject.name} HUD enabled for owner");
    }

    public void DisableHUD()
    {
        _hudView.gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} HUD disabled for owner");
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            UpdateHUDClientRpc(_currentHealth);
        }
    }

    public void Heal()
    {
        _currentHealth += 20;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        UpdateHUDClientRpc(_currentHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(bool isSelfDamage, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"{gameObject.name} took damage. Current health: {_currentHealth}");

        int currentDamage = isSelfDamage ? SELF_DAMAGE : GHOST_DAMAGE;

        _currentHealth -= currentDamage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth <= 0)
        {
            HandleDeathServerRpc();
        }
        else
        {
            UpdateHUDClientRpc(_currentHealth);
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

    [ServerRpc(RequireOwnership = false)]
    private void HandleDeathServerRpc()
    {

        DisableHUD();
        _bodyTransform.GetChild(0).gameObject.SetActive(false);

        _bodyRigidbody.isKinematic = true;
        _bodyRigidbody.useGravity = false;


        SpawnDeathEffect();
        HandleDeathClientRpc();
        UpdateHUDClientRpc(_currentHealth);
        Debug.Log($"{gameObject.name} entered spectator mode on client.");
    }

    [ClientRpc]
    private void HandleDeathClientRpc()
    {

        DisableHUD();
        _bodyTransform.gameObject.SetActive(false);

        _bodyRigidbody.isKinematic = true;
        _bodyRigidbody.useGravity = false;


        SpawnDeathEffect();

        Debug.Log($"{gameObject.name} entered spectator mode on client.");
    }

    private void SpawnDeathEffect()
    {
        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, transform.position, Quaternion.identity);
        }
    }

    private float GetHealthPercentage()
    {
        return _currentHealth / _maxHealth;
    }
}
