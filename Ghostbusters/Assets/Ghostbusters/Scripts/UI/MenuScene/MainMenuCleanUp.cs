using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (MultiplayerStorage.Instance != null)
        {
            Destroy(MultiplayerStorage.Instance.gameObject);
        }

        if (LobbyRelayManager.Instance != null)
        {
            Destroy(LobbyRelayManager.Instance.gameObject);
        }
    }
}
