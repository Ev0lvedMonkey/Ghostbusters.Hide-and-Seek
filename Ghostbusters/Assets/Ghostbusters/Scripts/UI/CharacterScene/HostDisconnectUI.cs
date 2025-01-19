using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;

    public void Init()
    {
        _playAgainButton.onClick.AddListener(BackToMenu);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    public void Uninit()
    {
        _playAgainButton.onClick.RemoveListener(BackToMenu);
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        Hide();
    }


    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                SceneLoader.Load(SceneLoader.ScenesEnum.MenuScene);
            }
            else
                Show();
        }
    }

    private static void BackToMenu()
    {
        ServiceLocator.Current.Get<LobbyRelayManager>().LeaveLobby();
        NetworkManager.Singleton.Shutdown();
        SceneLoader.Load(SceneLoader.ScenesEnum.MenuScene);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
