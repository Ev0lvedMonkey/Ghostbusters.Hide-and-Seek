using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class WorldSpaceCanvasTransform : NetworkBehaviour
{
    [SerializeField] private VirtualCameraPriority _camLoader;
    [SerializeField] private PlayerUI _playerUI;

    private Camera _camera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if(_camera == null)
            _camera = Camera.main;

        if (IsOwner)
        {
            string localPlayerName = ServiceLocator.Current.Get<PlayerSessionManager>().GetPlayerName();
            SetPlayerNameServerRpc(localPlayerName);
        }
    }

    private void Update()
    {
        RotateTowardCam(_camera);
    }

    private void RotateTowardCam(Camera cam)
    {
        transform.LookAt(transform.position + (cam.transform.rotation * Vector3.forward), cam. transform.rotation * Vector3.up);
    }

    [ServerRpc(RequireOwnership = true)]
    private void SetPlayerNameServerRpc(string playerName)
    {
        SetPlayerNameClientRpc(playerName);
    }

    [ClientRpc]
    private void SetPlayerNameClientRpc(string playerName)
    {
        _playerUI.SetPlayerName(playerName);
    }
}