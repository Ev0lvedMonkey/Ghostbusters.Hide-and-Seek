using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkObject))]
public class TransformableObject : NetworkBehaviour{

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Transformable");
        GetComponent<Rigidbody>().isKinematic = true;
    }
}