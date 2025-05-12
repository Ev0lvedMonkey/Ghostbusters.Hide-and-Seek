using Unity.Netcode;
using UnityEngine;

public class TransformingShoot : RayFiringObject
{
    [Header("Transforming Shoot Components")] 
    [SerializeField] private DeafaultCamPos _camPos;

    [SerializeField] private GameObject _bodyObject;
    [SerializeField] private GameObject _transformEffect;

    [Header("Audio Components")]
    [SerializeField] private AudioSource _audioSource;

    private MeshFilter _bodyMeshFilter;
    private MeshRenderer _bodyRenderer;
    private MeshCollider _bodyMeshCollider;
    private bool _firstTransformDone;

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
        if (!hit.collider.TryGetComponent<TransformableObject>(out TransformableObject transformableObject))
        {
            Debug.Log("TransformableObject not found");
            return;
        }

        if (!hit.transform.TryGetComponent<NetworkObject>(out NetworkObject targetNetworkObject))
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
            ApplyTransformClientRpc(targetObjectRef);
            PlaySoundClientRpc(transform.position);
        }
    }

    [ClientRpc]
    private void ApplyTransformClientRpc(NetworkObjectReference targetObjectRef)
    {
        if (!targetObjectRef.TryGet(out NetworkObject targetObject))
        {
            Debug.Log("Target object not found on client");
            return;
        }

        if (_transformEffect != null)
        {
            Instantiate(_transformEffect, transform.position + Vector3.up, Quaternion.identity);
        }

        Transform targetTransform = targetObject.transform;
        ApplyTransformLocally(targetTransform);
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
        if (!_firstTransformDone)
            _camPos.SetTransformedXCamPosition();
        _firstTransformDone = true;
    }

    [ClientRpc]
    private void PlaySoundClientRpc(Vector3 position)
    {
        _audioSource.transform.position = position;
        _audioSource.Play();
    }

    private void AdjustBodyPosition()
    {
        _bodyObject.transform.position =
            new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }
}