using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeponFire : MonoBehaviour
{
    [SerializeField, Range(0.02f, 3f)] private float _fireRate;
    [SerializeField, Range(1f, 250f)] private float _maxRayDistance;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private LayerMask _targetLayerMask;

    private float _fireRateTimer;

    private const KeyCode LMB = KeyCode.Mouse0;
    private const float ResetCooldownTime = 0;

    private void Start()
    {
        _fireRateTimer = _fireRate;
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

        if (_fireRateTimer < _fireRate) return false;

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
        if (Physics.Raycast(ray, out hit, _maxRayDistance, _targetLayerMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            MakeHitEffect(hit);
        }
    }

    private void MakeHitEffect(RaycastHit hit)
    {
        GameObject hitEffect = Instantiate(_hitEffect.gameObject, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(hitEffect, _hitEffect.duration);
    }
}
