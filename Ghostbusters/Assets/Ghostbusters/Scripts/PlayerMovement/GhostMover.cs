using UnityEngine;

public abstract class GhostMover : CharacterMover
{
    [SerializeField] private Transform _groundCheckDot;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;
    public bool IsRotationLocked { get; private set; }

    private readonly Vector3 JumpDir = Vector3.up;
    private const KeyCode JumpKey = KeyCode.Space;
    private const float JumpHeight = 5.5f;
    private const float GroundCheckRadius = 0.3f;

    protected virtual void Start()
    {
        GameStateManager.Instance.OnOpenHUD.AddListener(() => { IsRotationLocked = true; });
    }

    protected void GhostMove()
    {
        if (!IsOwner) return;

        TryJump();
        UpdateIdleState();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Move();
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
            IsRotationLocked = false;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            IsRotationLocked = true;
        }
    }

    protected virtual float GetModifiedSpeed()
    {
        return 1.0f; // ѕо умолчанию базовый множитель скорости
    }

    public override void Move()
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
}
