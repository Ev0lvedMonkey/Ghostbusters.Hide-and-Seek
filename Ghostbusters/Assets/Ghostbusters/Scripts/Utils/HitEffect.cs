using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
}
