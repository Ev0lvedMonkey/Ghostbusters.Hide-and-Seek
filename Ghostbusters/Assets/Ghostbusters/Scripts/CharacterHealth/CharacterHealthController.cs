using Unity.Netcode;
using UnityEngine;

public class CharacterHealthController : NetworkBehaviour
{
    private const int SelfDamage = 10;
    private const int GhostDamage = 20;

    [Header("Properties")]
    [SerializeField, Range(100, 150)] private float _maxHealth;

    [Header("Components")]
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Rigidbody _bodyRigidbody;
    [SerializeField] private RayFiringObject _fireObj;
    [SerializeField] private CharacterMover _characterMover;

    [Header("UI Elements")]
    [SerializeField] private HealthView _hudView;
    [SerializeField] private WorldSpaceCanvasTransform _spaceCanvasTransform;

    [Header("Audio")]
    [SerializeField] private AudioSource _deathAudioSource;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        ServiceLocator.Current.Get<GameOverWinUI>().PlayerExit.AddListener(() => { MomentKillClientRpc();});
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
        Debug.Log($"_currentHealth  {_currentHealth} _maxHealth {_maxHealth}");
        return _currentHealth < _maxHealth;
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
        int currentDamage = isSelfDamage ? SelfDamage : GhostDamage;

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
    private void MomentKillClientRpc()
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
            ulong clientID = ServiceLocator.Current.Get<PlayerSessionManager>().GetPlayerData().clientId;
            ServiceLocator.Current.Get<GameStateManager>().ReportPlayerLostServerRpc(clientID);
        }
        DisableHUD();
        _bodyTransform.gameObject.SetActive(false);
        _spaceCanvasTransform.gameObject.SetActive(false);

        _characterMover.enabled = false;
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
