using UnityEngine;

public abstract class RayFiringObject : MonoBehaviour
{
    [SerializeField, Range(0.02f, 3f)] protected float _fireRate;
    [SerializeField, Range(1f, 250f)] protected float _maxRayDistance;

    protected float _actionTimer;
    protected bool _isCooldownActive;

    protected const float ResetCooldownTimer = 0;
    protected const float HitCooldown = 3f;
    private const KeyCode LMB = KeyCode.Mouse0;

    protected virtual void Start()
    {
        _actionTimer = _fireRate;
        _isCooldownActive = false;
    }

    protected virtual void Update()
    {
        if (!CanFire())
            return;

        Fire();
    }

    protected bool CanFire()
    {
        _actionTimer += Time.deltaTime;

        if (_isCooldownActive)
        {
            if (_actionTimer >= HitCooldown)
            {
                _isCooldownActive = false;
                _actionTimer = _fireRate;
            }
            else
                return false;
        }

        return _actionTimer >= _fireRate && Input.GetKeyDown(LMB); ;
    }
    protected virtual void Fire()
    {
        ResetActionTimer();
        HandleFire();
    }

    protected void ResetActionTimer()
    {
        _actionTimer = ResetCooldownTimer;
    }

    protected void StartCooldown()
    {
        _actionTimer = ResetCooldownTimer;
        _isCooldownActive = true;
    }

    protected abstract void HandleFire();
}
