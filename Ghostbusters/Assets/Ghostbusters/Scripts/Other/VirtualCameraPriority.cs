using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class VirtualCameraPriority : NetworkBehaviour
{
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

    private void OnEnable()
    {
        if (IsOwner)
            _virtualCamera.Priority = 10;
        else
            _virtualCamera.Priority = 0;

    }
}
