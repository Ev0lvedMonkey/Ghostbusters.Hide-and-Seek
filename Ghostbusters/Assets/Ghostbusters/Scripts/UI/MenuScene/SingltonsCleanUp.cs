using Unity.Netcode;
using UnityEngine;

public class SingltonsCleanUp : MonoBehaviour
{
    public void CleanUp()
    {
        if (NetworkManager.Singleton != null)
            Destroy(NetworkManager.Singleton.gameObject);
        if (MultiplayerStorage.Instance != null)
            Destroy(MultiplayerStorage.Instance.gameObject);
        if (LobbyRelayManager.Instance != null)
            Destroy(LobbyRelayManager.Instance.gameObject);
    }
}
