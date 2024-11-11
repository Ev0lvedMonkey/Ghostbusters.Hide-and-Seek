using UnityEngine;

public class BlockingWall : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 20f;

    private bool _isOpen;

    void Start()
    {
        GameStateManager.Instance.OnStartGame.AddListener(() =>
        {
            Destroy(transform.gameObject, 1f);
        });
    }

    private void MoveWall()
    {
        Vector3 targetPosition = new(transform.position.x, transform.position.y * 2, transform.position.z);
        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }
}
