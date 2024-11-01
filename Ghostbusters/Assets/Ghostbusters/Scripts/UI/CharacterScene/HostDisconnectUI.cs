using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;

    private void Awake()
    {
        _playAgainButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.MenuScene);
        });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Show();
            }
            else
            {
                SceneLoader.Load(SceneLoader.Scene.MenuScene);
            }
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
