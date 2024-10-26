using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class WeponFire : RayFiringObject
{
    [SerializeField] private ParticleSystem _hitEffect;

    private GhostbusterHealthController _ghostbusterHealthController;
    private GhostHealthController _ghostHealthController;
    private LayerMask _targetLayerMask;
    private LayerMask _aghtungMask;
    private LayerMask _staticObjectsMask;

    private const string GhostLayer = "Ghost";
    private const string TransformableLayer = "Transformable";
    private const string StaticObjectsLayer = "StaticObjects";

    [Inject]
    private DiContainer _container;

    protected override void Start()
    {
        base.Start();
        _targetLayerMask = LayerMask.GetMask(GhostLayer);
        _aghtungMask = LayerMask.GetMask(TransformableLayer);
        _staticObjectsMask = LayerMask.GetMask(StaticObjectsLayer);
    }

    [System.Obsolete]
    protected override void HandleFire()
    {
        Transform firePosition = ServiceLocator.Current.Get<FirePositionService>().FirePosition;

        Ray ray = new(firePosition.position, firePosition.forward);

        Debug.DrawRay(firePosition.position, firePosition.forward * _maxRayDistance, Color.red, 3);


        if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance))
        {
            int hitLayer = hit.collider.gameObject.layer;

            if (((1 << hitLayer) & _targetLayerMask) != 0)
            {
                _ghostHealthController.TakeDamage();
                Debug.Log("Hit ghost object");
                StartCooldown();
            }
            else if (((1 << hitLayer) & _aghtungMask) != 0)
            {
                Debug.Log("Hit transformable object");
                _ghostbusterHealthController.TakeDamage();
            }
            else if (((1 << hitLayer) & _staticObjectsMask) != 0)
            {
                Debug.Log("Hit static object");
            }

            MakeHitEffect(hit);
        }
    }

    [System.Obsolete]
    private void MakeHitEffect(RaycastHit hit)
    {
        GameObject hitEffect = Instantiate(_hitEffect.gameObject, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(hitEffect, _hitEffect.duration);
        Debug.Log("Effect played");
    }

    [Inject]
    public void Construct(GhostbusterHealthController ghostbusterhealthController, GhostHealthController ghostHealthController)
    {
        _ghostbusterHealthController = ghostbusterhealthController;
        _ghostHealthController = ghostHealthController;
    }

    [Inject]
    public void InitializeSignal(SignalBus signalBus)
    {
        signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
    }

    private void OnPlayerDied()
    {
        Debug.Log($"try dead");
    }

}
