using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class  CharacterMover : NetworkBehaviour
{
    [SerializeField] private CharacterController _characterController;

    private Vector3 _direction;
    protected Vector2 _input;

    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const float BackMoveSpeed = 3.375f;
    private const float MovementSpeed = 4.5f;

    private void OnValidate()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();
    }


    public virtual void Move()
    {
        if (!isLocalPlayer) return;

        _input = GetInput();

        float horizontalInput = _input.x;
        float verticalInput = _input.y;

        _direction = transform.forward * verticalInput + transform.right * horizontalInput;

        _characterController.Move(_direction.normalized * GetMoveSpeed() * Time.deltaTime);

        //CmdUpdatePosition(transform.position);
    }

    private float GetMoveSpeed()
    {
        if (_input.y < 0)
            return BackMoveSpeed;
        else
            return MovementSpeed;
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }

    [Command]
    private void CmdUpdatePosition(Vector3 position)
    {
        RpcUpdatePosition(position);
    }

    [ClientRpc]
    private void RpcUpdatePosition(Vector3 position)
    {
        if (!isLocalPlayer)
        {
            transform.position = position; 
        }
    }
}
