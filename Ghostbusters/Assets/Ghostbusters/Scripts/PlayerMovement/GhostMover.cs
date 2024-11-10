using UnityEngine;

public class GhostMover : CharacterMover
{
    [SerializeField] internal Rigidbody _rigidbody;
    [SerializeField] private Transform _groundCheckDot;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;
    public bool IsRotationLocked { get; private set; } = false;

    private readonly Vector3 JumpDir = Vector3.up;
    private const KeyCode Jumpkey = KeyCode.Space;
    private const float JumpHeight = 5.5f;

    private void Update()
    {
        if (!IsOwner) return;

        TryJump();
        UpdateIdleState();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        base.Move();
    }

    private void TryJump()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckDot.position, 0.3f, _groundLayer);
        if (Input.GetKeyDown(Jumpkey) && _isGrounded)
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
}
