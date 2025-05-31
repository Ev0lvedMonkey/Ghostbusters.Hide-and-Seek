using UnityEngine;

public class GhostMover : CharacterMover
{
    private readonly Vector3 JumpDir = Vector3.up;
    private const KeyCode JumpKey = KeyCode.Space;
    private const float JumpHeight = 5.5f;
    private const float GroundCheckRadius = 0.3f;

    [Header("Ghost Movement Components")]
    [SerializeField] private Transform _groundCheckDot;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;

    public bool IsRotationLocked { get; private set; }

    protected override void Awake()
    {
        ServiceLocator.Current.Get<GameStateManager>().OnOpenHUD.AddListener(() => IsRotationLocked = true);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Move();
    }

    protected void GhostMove()
    {
        if (!IsOwner) return;

        TryJump();
        UpdateIdleState();
    }

    protected virtual float GetModifiedSpeed()
    {
        return 1.0f;
    }

    protected override void Move()
    {
        if (!IsOwner) return;

        _input = GetInput();

        float horizontalInput = _input.x;
        float verticalInput = _input.y;

        _direction = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 targetPosition = transform.position + _direction.normalized * GetMoveSpeed() * GetModifiedSpeed() * Time.deltaTime;
        _rigidbody.MovePosition(targetPosition);
        UpdatePositionServerRpc(targetPosition);
    }

    private void TryJump()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckDot.position, GroundCheckRadius, _groundLayer);
        if (Input.GetKeyDown(JumpKey) && _isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rigidbody.AddForce(JumpDir * JumpHeight, ForceMode.Impulse);
        _isGrounded = false;
        Debug.Log("Jumped");
    }

    private void UpdateIdleState()
    {
        if (_input != Vector2.zero)
        {
            if (ServiceLocator.Current.Get<GameOverWinUI>().IsOpened())
                return;
            IsRotationLocked = false;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            IsRotationLocked = true;
        }
    }

}
