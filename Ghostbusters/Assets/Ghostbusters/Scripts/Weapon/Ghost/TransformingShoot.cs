using Unity.Netcode;
using UnityEngine;

public class TransformingShoot : RayFiringObject
{
    [SerializeField] private GameObject _bodyObject;

    private MeshFilter bodyMeshFilter;
    private MeshRenderer bodyRenderer;
    private MeshCollider bodyMeshCollider;

    protected override void Start()
    {
        base.Start();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        bodyMeshFilter = _bodyObject.GetComponent<MeshFilter>();
        bodyRenderer = _bodyObject.GetComponent<MeshRenderer>();
        bodyMeshCollider = _bodyObject.GetComponent<MeshCollider>();
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

        ApplyTransformServerRpc(targetNetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyTransformServerRpc(NetworkObjectReference targetObjectRef)
    {
        if (targetObjectRef.TryGet(out NetworkObject targetObject))
        {
            Transform targetTransform = targetObject.transform;
            ApplyTransformLocally(targetTransform);

            ApplyTransformClientRpc(targetObjectRef);
        }
    }

    [ClientRpc]
    private void ApplyTransformClientRpc(NetworkObjectReference targetObjectRef)
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
        MeshCollider targetMeshCollider = targetTransform.GetComponent<MeshCollider>();

        if (targetMeshFilter == null && targetRenderer == null && targetMeshCollider == null)
            return;

        bodyMeshFilter.mesh = targetMeshFilter.mesh;
        bodyRenderer.materials = targetRenderer.materials;
        _bodyObject.transform.localScale = targetTransform.localScale;

        bodyMeshCollider.sharedMesh = targetMeshCollider.sharedMesh;
        bodyMeshCollider.convex = true;

        AdjustBodyPosition();
    }


    private void AdjustBodyPosition()
    {
        _bodyObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }
}
