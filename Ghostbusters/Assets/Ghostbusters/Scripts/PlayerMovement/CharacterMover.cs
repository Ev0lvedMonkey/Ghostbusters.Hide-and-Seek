using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public abstract class CharacterMover : NetworkBehaviour
{
    private Vector3 _direction;
    protected Vector2 _input;

    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const float BackMoveSpeed = 3.375f;
    [SerializeField] private float MovementSpeed = 4.5f;

    public virtual void Move()
    {
        if (!IsOwner) return;

        _input = GetInput();
        float horizontalInput = _input.x;
        float verticalInput = _input.y;

        _direction = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 targetPosition = transform.position + _direction.normalized * GetMoveSpeed() * Time.deltaTime;

        transform.position = targetPosition;
        UpdatePositionServerRpc(targetPosition);
    }

    [ServerRpc]
    private void UpdatePositionServerRpc(Vector3 targetPosition)
    {
        UpdatePositionClientRpc(targetPosition);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 targetPosition)
    {
        if (IsOwner) return;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }

    private float GetMoveSpeed()
    {
        return _input.y < 0 ? BackMoveSpeed : MovementSpeed;
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }
}
