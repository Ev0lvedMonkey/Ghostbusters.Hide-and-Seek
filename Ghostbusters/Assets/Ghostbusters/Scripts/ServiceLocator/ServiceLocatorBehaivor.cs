using UnityEngine;

public class ServiceLocatorBehaivor : MonoBehaviour
{
    [SerializeField] private FirePositionService _firePositionService;

    private void Awake()
    {
        RegisterServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Inizialize();
        ServiceLocator.Current.Register(_firePositionService);
    }
}
