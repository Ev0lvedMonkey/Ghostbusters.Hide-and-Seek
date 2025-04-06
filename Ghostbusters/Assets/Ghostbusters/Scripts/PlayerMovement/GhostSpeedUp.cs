using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GhostSpeedUp : GhostMover
{
    [SerializeField] private UnityEvent OnEnableAbility;
    [SerializeField] private UnityEvent OnDisableAbility;


    private const float SpeedBoostMultiplier = 1.75f;
    private const float SpeedBoostDuration = 4f;
    private const float SpeedBoostCooldown = 9f;

    private bool _isSpeedBoostActive;
    private bool _canUseSpeedBoost = true;

    protected override void Start()
    {
        base.Start();
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

    protected override float GetModifiedSpeed()
    {
        return _isSpeedBoostActive ? SpeedBoostMultiplier : 1.0f;
    }
}
