using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GhostSpeedUp : GhostMover
{
    private const float SpeedBoostMultiplier = 1.4f;
    private const float SpeedBoostDuration = 4f;
    private const float SpeedBoostCooldown = 9f;

    [Header("Ghost Speed Up Components")]
    [SerializeField] protected CharacterAbilityStateChanger _characterAbilityStateChanger;

    private bool _isSpeedBoostActive;
    private bool _canUseSpeedBoost = true;

    private readonly UnityEvent OnEnableAbility = new();
    private readonly UnityEvent OnDisableAbility = new();

    protected override void Awake()
    {
        base.Awake();
        OnEnableAbility.AddListener(_characterAbilityStateChanger.EnableAbility);
        OnDisableAbility.AddListener(_characterAbilityStateChanger.DisableAbility);
    }
    
    private void Update()
    {
        base.Update();
        if (!IsOwner) return;

        GhostMove();

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canUseSpeedBoost)
        {
            ActivateSpeedBoost();
        }
    }

    protected override float GetModifiedSpeed()
    {
        return _isSpeedBoostActive ? SpeedBoostMultiplier : 1.0f;
    }

    private void ActivateSpeedBoost()
    {
        if (_isSpeedBoostActive) return;
        OnDisableAbility.Invoke();
        _isSpeedBoostActive = true;
        _canUseSpeedBoost = false;
        StartCoroutine(SpeedBoostCoroutine());
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        yield return new WaitForSeconds(SpeedBoostDuration);

        _isSpeedBoostActive = false;

        yield return new WaitForSeconds(SpeedBoostCooldown - SpeedBoostDuration);
        Debug.Log("Speed Boost Ready");
        _canUseSpeedBoost = true;
        OnEnableAbility.Invoke();
    }
}
