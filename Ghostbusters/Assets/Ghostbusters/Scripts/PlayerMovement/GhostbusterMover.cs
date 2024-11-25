using Unity.Netcode;
using UnityEngine;

public class GhostbusterMover : CharacterMover
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _blockingSphere;
    [SerializeField] private GameObject _blockingSphereSpawnEffect;

    private bool _isWasCreate;

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnSphereServerRpc();
            Instantiate(_blockingSphere, transform.position, Quaternion.identity);
            _isWasCreate = true;
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