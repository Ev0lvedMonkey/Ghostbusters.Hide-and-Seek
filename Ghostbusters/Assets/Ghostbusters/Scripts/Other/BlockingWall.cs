using Org.BouncyCastle.Security.Certificates;
using System.Collections;
using UnityEngine;

public class BlockingWall : MonoBehaviour
{
    [SerializeField] private Transform _stopPos;
    [SerializeField] private float _moveSpeed = 8f;

    private bool _isOpen;

    void Start()
    {
        GameStateManager.Instance.OnStartGame.AddListener(() => { _isOpen = true; Destroy(transform.gameObject, 3f);
        });
    }

    private void MoveWall()
    {
        Vector3 targetPosition = new(transform.position.x, transform.position.y * 2, transform.position.z);
        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }

    void Update()
    {
        if (_isOpen)
        {
            MoveWall();
        }
    }
}
