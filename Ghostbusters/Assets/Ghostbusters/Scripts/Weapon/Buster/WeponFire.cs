using Unity.Netcode;
using UnityEngine;

public class WeaponFire : RayFiringObject
{
    private const string GhostLayer = "Ghost";
    private const string TransformableLayer = "Transformable";
    private const string StaticObjectsLayer = "StaticObjects";

    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private CharacterHealthController _characterHealth;
    [SerializeField] private AudioSource _shootSource;

    private LayerMask _targetLayerMask;
    private LayerMask _transformableLayerMask;
    private LayerMask _staticObjectsLayerMask;

    protected override void Start()
    {
        base.Start();
        _targetLayerMask = LayerMask.GetMask(GhostLayer);
        _transformableLayerMask = LayerMask.GetMask(TransformableLayer);
        _staticObjectsLayerMask = LayerMask.GetMask(StaticObjectsLayer);
    }

    private void OnValidate()
    {
        if (_characterHealth == null)
        {
            if (TryGetComponent(out CharacterHealthController controller))
                _characterHealth = controller;
        }
    }

    protected override void HandleFire()
    {
        Transform firePosition = ServiceLocator.Current.Get<FirePositionService>().FirePosition;

        Ray ray = new(firePosition.position, firePosition.forward);

        Debug.DrawRay(firePosition.position, firePosition.forward * _maxRayDistance, Color.red, 3);

        _shootSource.Play();
        if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance))
        {
            int hitLayer = hit.collider.gameObject.layer;

            if (((1 << hitLayer) & _targetLayerMask) != 0)
            {
                if (hit.collider.TryGetComponent(out CharacterHealthController controller))
                {
                    controller.TakeDamageServerRpc(false);
                    Debug.Log("Hit ghost object");
                }
                StartCooldown();
            }
            else if (((1 << hitLayer) & _transformableLayerMask) != 0)
            {
                Debug.Log("Hit transformable object");
                _characterHealth.TakeDamageServerRpc(true);
            }
            else if (((1 << hitLayer) & _staticObjectsLayerMask) != 0)
            {
                Debug.Log("Hit static object");
            }
            Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));

            MakeHitEffectServerRpc(hit.point, hit.normal);
        }
    }


    [ServerRpc]
    private void MakeHitEffectServerRpc(Vector3 hitPoint, Vector3 hitNormal)
    {
        MakeHitEffectClientRpc(hitPoint, hitNormal);
    }

    [ClientRpc]
    private void MakeHitEffectClientRpc(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsOwner) return;
        Instantiate(_hitEffect, hitPoint, Quaternion.LookRotation(hitNormal));
    }
}
