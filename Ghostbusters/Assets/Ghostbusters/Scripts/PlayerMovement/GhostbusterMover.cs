using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GhostbusterMover : CharacterMover
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    
    [Header("Сomponents")]
    [SerializeField] private GameObject _blockingSphere;
    [SerializeField] private GameObject _blockingSphereSpawnEffect;
    [SerializeField] private CharacterAbilityStateChanger _characterAbilityStateChanger;
    
    private readonly UnityEvent OnDisableAbility = new();

    protected override void Awake()
    {
        base.Awake();
        OnDisableAbility.AddListener(_characterAbilityStateChanger.DisableAbility);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (ServiceLocator.Current.Get<GameOverWinUI>().IsOpened()) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnSphereServerRpc();
            SpawnEffectServerRpc();
            OnDisableAbility.Invoke();
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
        var instance = Instantiate(_blockingSphere, transform.position, Quaternion.identity);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
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