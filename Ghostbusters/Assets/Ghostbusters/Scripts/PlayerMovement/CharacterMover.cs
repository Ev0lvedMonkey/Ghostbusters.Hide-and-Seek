using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterMover : NetworkBehaviour
{
    [SerializeField] internal Rigidbody _rigidbody;
    protected Vector3 _direction;
    protected Vector2 _input;
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const float BackMoveSpeed = 3.375f;
    private const float MovementSpeed = 4.5f;

    private void Awake()
    {
        _rigidbody.isKinematic = true;
    }

    public virtual void Move()
    {
        if (!IsOwner) return;

        _input = GetInput();

        float horizontalInput = _input.x;
        float verticalInput = _input.y;

        _direction = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 targetPosition = transform.position + _direction.normalized * GetMoveSpeed() * Time.deltaTime;
        _rigidbody.MovePosition(targetPosition);
        UpdatePositionServerRpc(targetPosition);
    }

    [ServerRpc]
    protected void UpdatePositionServerRpc(Vector3 targetPosition)
    {
        UpdatePositionClientRpc(targetPosition);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 targetPosition)
    {
        if (IsOwner) return;
        _rigidbody.MovePosition(targetPosition);
    }

    protected float GetMoveSpeed()
    {
        return _input.y < 0 ? BackMoveSpeed : MovementSpeed;
    }

    protected Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }
}
