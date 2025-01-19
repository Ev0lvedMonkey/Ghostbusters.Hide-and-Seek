using Unity.Netcode;
using UnityEngine;

public class TransformingShoot : RayFiringObject
{
    [SerializeField] private GameObject _bodyObject;
    [SerializeField] private AudioSource _audioSource;

    private MeshFilter _bodyMeshFilter;
    private MeshRenderer _bodyRenderer;
    private MeshCollider _bodyMeshCollider;

    protected override void Start()
    {
        base.Start();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _bodyMeshFilter = _bodyObject.GetComponent<MeshFilter>();
        _bodyRenderer = _bodyObject.GetComponent<MeshRenderer>();
        _bodyMeshCollider = _bodyObject.GetComponent<MeshCollider>();
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

        _bodyMeshFilter.mesh = targetMeshFilter.mesh;
        _bodyRenderer.materials = targetRenderer.materials;
        _bodyObject.transform.localScale = targetTransform.localScale;

        _bodyMeshCollider.sharedMesh = targetMeshCollider.sharedMesh;
        _bodyMeshCollider.convex = true;

        AdjustBodyPosition();
        _audioSource.Play();
    }


    private void AdjustBodyPosition()
    {
        _bodyObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }
}
