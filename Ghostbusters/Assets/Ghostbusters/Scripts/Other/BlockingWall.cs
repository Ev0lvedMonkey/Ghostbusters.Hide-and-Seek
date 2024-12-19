using UnityEngine;

public class BlockingWall : MonoBehaviour
{
    void Start()
    {
        GameStateManager.Instance.OnStartGame.AddListener(() =>
        {
            Destroy(transform.gameObject);
        });
    }
}
