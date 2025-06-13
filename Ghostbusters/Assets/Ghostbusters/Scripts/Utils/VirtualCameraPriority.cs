using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class VirtualCameraPriority : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void OnValidate()
    {
        if (_virtualCamera != null)
            return;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out CinemachineVirtualCamera virtualCamera))
                _virtualCamera = virtualCamera;
        }
    }

    private void Start()
    {
        if (IsOwner)
        {
            _virtualCamera.Priority = 10;
            ActivateCamera();
        }
        else
        {
            DeactivateCamera();
            _virtualCamera.Priority = 0;
        }

    }
    
    private void ActivateCamera() =>
            _virtualCamera.gameObject.SetActive(true);

    private void DeactivateCamera() =>
            _virtualCamera.gameObject.SetActive(false);
}
