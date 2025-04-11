using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterMover : NetworkBehaviour
{
    [Header("Character Mover Componets")]
    [SerializeField] internal Rigidbody _rigidbody;
    [SerializeField] internal Transform _navCheckDot;

    private Vector3 _safePoint;
    private float _timer;
    protected Vector3 _direction;
    protected Vector2 _input;

    
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const float CheckRadius = 0.5f;
    private const float CheckInterval = 2.5f;
    private const float BackMoveSpeed = 3.375f;
    private const float MovementSpeed = 4.5f;

    protected virtual void Awake()
    {
        _rigidbody.isKinematic = true;
    }

    protected virtual void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= CheckInterval)
        {
            _timer = 0f;
            if (!IsOnNavMesh())
            {
                Debug.LogError("Out of bounds");
                transform.position = _safePoint;
                _rigidbody.position = _safePoint;
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                _safePoint = new(transform.position.x, transform.position.y + 1.2f, transform.position.z); 
            }
        }
    }

    protected virtual void Move()
    {
        if (!IsOwner) return;

        _input = GetInput();

        float horizontalInput = _input.x;
        float verticalInput = _input.y;

        _direction = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 targetPosition = transform.position + _direction.normalized * (GetMoveSpeed() * Time.deltaTime);
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
        if (ServiceLocator.Current.Get<GameOverWinUI>().IsOpened()
            || ServiceLocator.Current.Get<ChatManager>().IsOpened()) return 0f;
        else return _input.y < 0 ? BackMoveSpeed : MovementSpeed;
    }

    protected Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }

    private bool IsOnNavMesh()
    {
        return NavMesh.SamplePosition(new Vector3(_navCheckDot.position.x, 0f, _navCheckDot.position.z ), out _, CheckRadius, NavMesh.AllAreas);
    }
}