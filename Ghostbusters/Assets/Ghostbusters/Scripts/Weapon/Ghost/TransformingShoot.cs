using Unity.Netcode;
using UnityEngine;

public class TransformingShoot : RayFiringObject
{
    [SerializeField] private GameObject _bodyObject;

    private MeshFilter bodyMeshFilter;
    private MeshRenderer bodyRenderer;
    private CapsuleCollider bodyCapsuleCollider;

    protected override void Start()
    {
        base.Start();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        bodyMeshFilter = _bodyObject.GetComponent<MeshFilter>();
        bodyRenderer = _bodyObject.GetComponent<MeshRenderer>();
        bodyCapsuleCollider = _bodyObject.GetComponent<CapsuleCollider>();
    }

    protected override void HandleFire()
    {
        Transform firePosition = ServiceLocator.Current.Get<FirePositionService>().FirePosition;

        Ray ray = new(firePosition.position, firePosition.forward);

        Debug.DrawRay(firePosition.position, firePosition.forward * _maxRayDistance, Color.red, 3);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance))
        {
            TryTransform(hit);
            StartCooldown();
        }
    }

    private void TryTransform(RaycastHit hit)
    {
        if (!hit.collider.TryGetComponent<TransformableObject>(out var transformableObject))
        {
            Debug.Log("TransformableObject not found");
            return;
        }

        if (!hit.transform.TryGetComponent<NetworkObject>(out var targetNetworkObject))
        {
            Debug.Log("NetworkObject not found");
            return;
        }

        ApplyTransformServerRpc(targetNetworkObject, OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyTransformServerRpc(NetworkObjectReference targetObjectRef, ulong clientId)
    {
        if (targetObjectRef.TryGet(out NetworkObject targetObject))
        {
            Transform targetTransform = targetObject.transform;
            ApplyTransformLocally(targetTransform);

            ApplyTransformClientRpc(targetObjectRef, clientId);
        }
    }

    [ClientRpc]
    private void ApplyTransformClientRpc(NetworkObjectReference targetObjectRef, ulong initiatorClientId)
    {
        if (targetObjectRef.TryGet(out NetworkObject targetObject))
        {
            Transform targetTransform = targetObject.transform;
            ApplyTransformLocally(targetTransform);
        }
    }

    private void ApplyTransformLocally(Transform targetTransform)
    {
        MeshFilter targetMeshFilter = targetTransform.GetComponent<MeshFilter>();
        MeshRenderer targetRenderer = targetTransform.GetComponent<MeshRenderer>();
        CapsuleCollider targetCapsuleCollider = targetTransform.GetComponent<CapsuleCollider>();

        if (targetMeshFilter == null && targetRenderer == null && targetCapsuleCollider == null)
            return;

        bodyMeshFilter.mesh = targetMeshFilter.mesh;
        bodyRenderer.materials = targetRenderer.materials;
        _bodyObject.transform.localScale = targetTransform.localScale;

        bodyCapsuleCollider.center = targetCapsuleCollider.center;
        bodyCapsuleCollider.radius = targetCapsuleCollider.radius;
        bodyCapsuleCollider.height = targetCapsuleCollider.height;

        AdjustBodyPosition();
    }

    private void AdjustBodyPosition()
    {
        _bodyObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }
}
