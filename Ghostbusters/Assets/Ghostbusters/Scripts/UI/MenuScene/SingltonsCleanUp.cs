using Unity.Netcode;
using UnityEngine;

public class SingltonsCleanUp : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    public void CleanUp()
    {
        _serviceLocator = ServiceLocator.Current;
        if (NetworkManager.Singleton != null)
            Destroy(NetworkManager.Singleton.gameObject);
        if (_serviceLocator != null)
        {
            if (_serviceLocator.Get<MultiplayerStorage>() != null)
                Destroy(_serviceLocator.Get<MultiplayerStorage>().gameObject);
            if (_serviceLocator.Get<LobbyRelayManager>() != null)
                Destroy(_serviceLocator.Get<LobbyRelayManager>().gameObject);
            if (_serviceLocator.Get<AudioManager>() != null)
                Destroy(_serviceLocator.Get<AudioManager>().gameObject);
            _serviceLocator.MakeNull();
        }
    }
}
