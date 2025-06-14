using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterHealthController : NetworkBehaviour
{
    private const int SelfDamage = 10;
    private const int GhostDamage = 20;

    [Header("Properties")] [SerializeField, Range(100, 150)]
    private float _maxHealth;

    [Header("Components")] [SerializeField]
    private GameObject _deathEffect;

    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Rigidbody _bodyRigidbody;
    [SerializeField] private RayFiringObject _fireObj;
    [SerializeField] private CharacterMover _characterMover;

    [Header("UI Elements")] [SerializeField]
    private HealthView _hudView;

    [Header("Audio")] [SerializeField] private AudioSource _deathAudioSource;

    private float _currentHealth;


    public UnityAction OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        ServiceLocator.Current.Get<GameOverWinUI>().PlayerExit.AddListener(() => { MomentKillClientRpc(); });
    }

    private void Start()
    {
        if (!IsOwner)
            DisableHUD();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            TakeDamageServerRpc(true);
    }

    public override void OnNetworkSpawn()
    {
        UpdateHUDClientRpc(_currentHealth);
    }

    public bool IsNeedHealth()
    {
        Debug.Log($"_currentHealth  {_currentHealth} _maxHealth {_maxHealth} need {_currentHealth < _maxHealth}");
        return _currentHealth < _maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    public void HealServerRpc()
    {
        HealClientRpc();
    }

    [ClientRpc]
    private void HealClientRpc()
    {
        _currentHealth += 20;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log($"Got heal {_currentHealth}");

        UpdateHUDClientRpc(_currentHealth);
    }

    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(bool isSelfDamage, ServerRpcParams rpcParams = default)
    {
        int currentDamage = isSelfDamage ? SelfDamage : GhostDamage;

        _currentHealth -= currentDamage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (IsDead())
        {
            HandleDeathServerRpc();
        }
        else
        {
            UpdateHUDClientRpc(_currentHealth);
        }
    }

    [ClientRpc]
    private void MomentKillClientRpc()
    {
        _currentHealth -= _maxHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (IsDead())
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
        Debug.Log($"{gameObject.name} get new health {newHealth} {_currentHealth}");

        if (_hudView != null)
        {
            _hudView.UpdateHealthBar(GetHealthPercentage(), _currentHealth);
            if (IsOwner)
            {
                Debug.Log($"{gameObject.name} updated HUD on owner client with health {_currentHealth}");
            }
        }
        else
            Debug.Log($"_hudView null {_hudView == null}");
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleDeathServerRpc()
    {
        HandleDeathClientRpc();
    }

    [ClientRpc]
    protected void HandleDeathClientRpc()
    {
        if (IsOwner)
        {
            ulong clientID = ServiceLocator.Current.Get<PlayerSessionManager>().GetPlayerData().clientId;
            ServiceLocator.Current.Get<GameStateManager>().ReportPlayerLostServerRpc(clientID);
            ServiceLocator.Current.Get<ChatManager>().SetOwnerDead();
        }

        DisableHUD();
        _bodyTransform.gameObject.SetActive(false);
        BusterUniqMove();

        _characterMover.enabled = false;
        _bodyRigidbody.isKinematic = true;
        _bodyRigidbody.useGravity = false;
        _fireObj.enabled = false;
        SpawnDeathEffectClientRpc();
        UpdateHUDClientRpc(_currentHealth);
        OnDeath.Invoke();
        Debug.Log($"{gameObject.name} entered spectator mode on client.");
    }

    protected abstract void BusterUniqMove();

    [ClientRpc]
    private void SpawnDeathEffectClientRpc()
    {
        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
                Quaternion.identity);
            _deathAudioSource.Play();
        }
    }

    private void DisableHUD()
    {
        _hudView.gameObject.SetActive(false);
    }

    private float GetHealthPercentage()
    {
        return _currentHealth / _maxHealth;
    }
}