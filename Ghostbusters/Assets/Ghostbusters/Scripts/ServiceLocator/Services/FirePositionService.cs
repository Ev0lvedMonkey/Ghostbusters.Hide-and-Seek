using UnityEngine;

public class FirePositionService : MonoBehaviour, IService
{
    public Transform FirePosition { get; private set; }

    private void OnValidate()
    {
        if (FirePosition == null)
            FirePosition = transform;
    }

    void Start()
    {
        FirePosition = transform;
    }
}
