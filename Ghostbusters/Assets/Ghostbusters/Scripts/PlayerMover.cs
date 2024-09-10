using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;

    private Vector3 _direction;
    private Vector2 _input;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const float BackMoveSpeed = 3.375f;
    private const float MovementSpeed = 4.5f;

    private void OnValidate()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _input = GetInput();

        float horizontalInput = _input.x;
        float verticalInput = _input.y;

        _direction = transform.forward * verticalInput + transform.right * horizontalInput;

        SetAnimState();

        _characterController.Move(_direction.normalized * GetMoveSpeed() * Time.deltaTime);
    }

    private float GetMoveSpeed()
    {
        if (_input.y < 0)
            return BackMoveSpeed;
        else
            return MovementSpeed;
    }

    private void SetAnimState()
    {
        _animator.SetFloat(Horizontal, _input.x);
        _animator.SetFloat(Vertical, _input.y);
    }

    private Vector2 GetInput()
    {
        return new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
}
