using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GhostbusterMover : CharacterMover
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    
    [Header("Components")]
    [SerializeField] private GameObject _blockingSphere;
    [SerializeField] private GameObject _blockingSphereSpawnEffect;
    [SerializeField] private CharacterAbilityStateChanger _characterAbilityStateChanger;

    private bool _abilityUsed;
    private GameOverWinUI _gameOverWinUI;

    private readonly UnityEvent OnDisableAbility = new();

    protected override void Awake()
    {
        base.Awake();
        _gameOverWinUI = ServiceLocator.Current.Get<GameOverWinUI>();
        OnDisableAbility.AddListener(_characterAbilityStateChanger.DisableAbility);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (_gameOverWinUI.IsOpened()) return;
        if (Input.GetKeyDown(KeyCode.E) && !_abilityUsed)
        {
            if(ServiceLocator.Current.Get<ChatManager>().IsOpened())
                return;
            SpawnSphereServerRpc();
            SpawnEffectServerRpc();
            OnDisableAbility.Invoke();
            _abilityUsed = true;
        }
        SetAnimState();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        base.Move();
    }

    private void SetAnimState()
    {
        _animator.SetFloat(Horizontal, _input.x);
        _animator.SetFloat(Vertical, _input.y);
    }

    [ServerRpc]
    private void SpawnSphereServerRpc()
    {
        GameObject instance = Instantiate(_blockingSphere, transform.position, Quaternion.identity);
        NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn(true);
    }

    [ServerRpc]
    private void SpawnEffectServerRpc()
    {
        SpawnEffectClientRpc();
    }

    [ClientRpc]
    private void SpawnEffectClientRpc()
    {
        Instantiate(_blockingSphereSpawnEffect, transform.position, Quaternion.identity);
    }
}