using System;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class WeponFire : MonoBehaviour
{
    [SerializeField, Range(0.02f, 3f)] private float _fireRate;
    [SerializeField, Range(1f, 250f)] private float _maxRayDistance;
    [SerializeField] private ParticleSystem _hitEffect;

    private GhostbusterHealthController _ghostbusterHealthController;
    private GhostHealthController _ghostHealthController;
    private float _fireRateTimer;
    private float _cooldownTimer;
    private LayerMask _targetLayerMask;
    private LayerMask _aghtungMask;
    private LayerMask _staticObjectsMask;

    private const KeyCode LMB = KeyCode.Mouse0;
    private const float ResetCooldownTime = 0;
    private const float HitCooldown = 3f;

    private const string GhostLayer = "Ghost";
    private const string TransformableLayer = "Transformable";
    private const string StaticObjectsLayer = "StaticObjects";


    private void Start()
    {
        _fireRateTimer = _fireRate;
        _cooldownTimer = HitCooldown;

        _targetLayerMask = LayerMask.GetMask(GhostLayer);
        _aghtungMask = LayerMask.GetMask(TransformableLayer);
        _staticObjectsMask = LayerMask.GetMask(StaticObjectsLayer);
    }

    private void Update()
    {
        if (!ShouldFire())
            return;
        Fire();
    }


    private bool ShouldFire()
    {
        _fireRateTimer += Time.deltaTime;
        _cooldownTimer += Time.deltaTime;

        if (_fireRateTimer < _fireRate) return false;
        if (_cooldownTimer < HitCooldown) return false;
        if (Input.GetKeyDown(LMB)) return true;

        return false;
    }

    private void Fire()
    {
        _fireRateTimer = ResetCooldownTime;

        Transform firePosition = ServiceLocator.Current.Get<FirePositionService>().FirePosition;

        Ray ray = new(firePosition.position, firePosition.forward);
        RaycastHit hit;

        Debug.DrawRay(firePosition.position, firePosition.forward * _maxRayDistance, Color.red, 3);

        int combinedMask = _targetLayerMask | _aghtungMask | _staticObjectsMask;

        if (Physics.Raycast(ray, out hit, _maxRayDistance, combinedMask))
        {
            int hitLayer = hit.collider.gameObject.layer;

            if (((1 << hitLayer) & _targetLayerMask) != 0)
            {
                _ghostHealthController.TakeDamage();
                Debug.Log("Hit ghost object");
                _cooldownTimer = 0;
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
    public void InitializeSignal (SignalBus signalBus)
    {
        signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
    }

    private void OnPlayerDied()
    {
        Debug.Log($"try dead");
    }
}
