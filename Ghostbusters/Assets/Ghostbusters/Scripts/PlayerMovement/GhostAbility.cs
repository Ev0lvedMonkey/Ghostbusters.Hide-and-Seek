using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class GhostAbility : NetworkBehaviour
{
    private const float SpeedBoostDuration = 6f;
    private const float SpeedBoostCooldown = 25f;

    [Header("Ghost Speed Up Components")]
    [SerializeField] private CharacterAbilityStateChanger _characterAbilityStateChanger;
    [SerializeField] private GameObject _abilityEffect;

    private bool _abilityIsActive;
    private bool _canUseAbility = true;

    private readonly UnityEvent OnEnableAbility = new();
    private readonly UnityEvent OnDisableAbility = new();

    protected void Awake()
    {
        OnEnableAbility.AddListener(_characterAbilityStateChanger.EnableAbility);
        OnDisableAbility.AddListener(_characterAbilityStateChanger.DisableAbility);
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canUseAbility)
        {
            ActivateAbilityServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ActivateAbilityServerRpc()
    {
        ActivateAbilityClientRpc();
    }

    [ClientRpc]
    private void ActivateAbilityClientRpc()
    {
        if (_abilityIsActive) return;
        OnDisableAbility.Invoke();
        Instantiate(_abilityEffect, transform.position + Vector3.up, Quaternion.identity);
        _abilityIsActive = true;
        _canUseAbility = false;
        StartCoroutine(AbilityBoostCoroutine());
    }

    private IEnumerator AbilityBoostCoroutine()
    {
        yield return new WaitForSeconds(SpeedBoostDuration);

        _abilityIsActive = false;

        yield return new WaitForSeconds(SpeedBoostCooldown - SpeedBoostDuration);
        Debug.Log("Ability Ready");
        _canUseAbility = true;
        OnEnableAbility.Invoke();
    }
}