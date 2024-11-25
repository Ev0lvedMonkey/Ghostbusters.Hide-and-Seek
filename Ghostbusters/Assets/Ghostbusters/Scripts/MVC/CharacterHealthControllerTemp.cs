using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealthControllerTemp : NetworkBehaviour
{
    [SerializeField, Range(100, 150)] private float _maxHealth;
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Rigidbody _bodyRigidbody;
    [SerializeField] private RayFiringObject _fireObj;
    [SerializeField] private UnityEvent OnSpectatorStart;
    [SerializeField] private HealthView _hudView;

    private const int SELF_DAMAGE = 10;
    private const int GHOST_DAMAGE = 20;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        GameOverWinUI.Instance.PlayerExit.AddListener(() => { MomentKillClientRpc();});
    }

    private void Start()
    {
        if (IsOwner)
            EnableHUD();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            TakeDamageServerRpc(true);
    }

    public void EnableHUD()
    {
        _hudView.gameObject.SetActive(true);
    }

    public void DisableHUD()
    {
        _hudView.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        UpdateHUDClientRpc(_currentHealth);
    }

    public void Heal()
    {
        _currentHealth += 20;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log($"Health UPPPPP");

        UpdateHUDClientRpc(_currentHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(bool isSelfDamage, ServerRpcParams rpcParams = default)
    {
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
    public void MomentKillClientRpc()
    {
        _currentHealth -= _maxHealth;
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
        HandleDeathClientRpc();
    }

    [ClientRpc]
    private void HandleDeathClientRpc()
    {
        if (IsOwner)
        {
            ulong clientID = MultiplayerStorage.Instance.GetPlayerData().clientId;
            GameStateManager.Instance.ReportPlayerLostServerRpc(clientID);
        }
        DisableHUD();
        _bodyTransform.gameObject.SetActive(false);

        OnSpectatorStart.Invoke();
        _bodyRigidbody.isKinematic = true;
        _bodyRigidbody.useGravity = false;
        _fireObj.enabled = false;
        SpawnDeathEffectClientRpc();
        UpdateHUDClientRpc(_currentHealth);
        Debug.Log($"{gameObject.name} entered spectator mode on client.");
    }

    [ClientRpc]
    private void SpawnDeathEffectClientRpc()
    {
        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        }
    }

    private float GetHealthPercentage()
    {
        return _currentHealth / _maxHealth;
    }
}
