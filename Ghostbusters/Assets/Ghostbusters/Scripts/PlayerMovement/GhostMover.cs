using UnityEngine;

public class GhostMover : CharacterMover
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _groundCheckDot;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;

    private readonly Vector3 JumpDir = Vector3.up;
    private const KeyCode Jumpkey = KeyCode.Space;
    private const float JumpHeight = 5.5f;

    private void Update()
    {
        if (!isLocalPlayer) return;
        TryJump();
        base.Move();
    }

    private void TryJump()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckDot.position, 0.1f, _groundLayer);
        Debug.Log($"Grounded: {_isGrounded}");

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
}
