using UnityEngine;
using Unity.Netcode;

public abstract class RayFiringObject : NetworkBehaviour
{
    private const float ResetCooldownTimer = 0;
    private const float HitCooldown = 1.5f;
    private const KeyCode LMB = KeyCode.Mouse0;

    [Header("Base Components")]
    [SerializeField, Range(0.02f, 3f)] protected float _fireRate;
    [SerializeField, Range(1f, 250f)] protected float _maxRayDistance;

    private float _actionTimer;
    private bool _isCooldownActive;
    private GameOverWinUI _gameOverWinUI;
    private ChatManager _chatManager;
    
    protected virtual void Start()
    {
        _gameOverWinUI = ServiceLocator.Current.Get<GameOverWinUI>();
        _chatManager = ServiceLocator.Current.Get<ChatManager>();
        _actionTimer = _fireRate;
        _isCooldownActive = false;
    }

    protected virtual void Update()
    {
        if (!IsOwner || !CanFire())
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

        bool menuOpened = _gameOverWinUI.IsOpened() || _chatManager.IsOpened();
        
        return _actionTimer >= _fireRate && !menuOpened && Input.GetKeyDown(LMB);
    }

    protected virtual void Fire()
    {
        ResetActionTimer();
        HandleFire();
    }

    protected void ResetActionTimer() =>
        _actionTimer = ResetCooldownTimer;    

    protected void StartCooldown()
    {
        _actionTimer = ResetCooldownTimer;
        _isCooldownActive = true;
    }
   
    protected abstract void HandleFire();
}
