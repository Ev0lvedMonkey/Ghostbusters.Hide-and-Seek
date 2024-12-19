using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;

    public void Init()
    {
        _playAgainButton.onClick.AddListener(() =>
            SceneLoader.Load(SceneLoader.Scene.MenuScene));
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    public void Uninit()
    {
        _playAgainButton.onClick.RemoveListener(() =>
            SceneLoader.Load(SceneLoader.Scene.MenuScene));
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        Hide();
    }


    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (NetworkManager.Singleton.IsHost)
                SceneLoader.Load(SceneLoader.Scene.MenuScene);
            else
                Show();
        }
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
