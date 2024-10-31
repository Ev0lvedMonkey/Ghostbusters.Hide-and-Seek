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
        Debug.LogError($"CLIENT ID {clientId}");
        PlayerData player = MultiplayerStorage.Instance.GetPlayerDataFromClientId(clientId);
        Debug.LogError($"player ID {player.playerId}");
        Debug.LogError($"player name {player.playerName}");


        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (NetworkManager.Singleton.IsHost)
            {

                Debug.LogError("NetworkManager_OnClientDisconnectCallback");
                Show();
            }
            else
            {
                SceneLoader.Load(SceneLoader.Scene.MenuScene);
                Debug.LogError("KKKKKKKKK");
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
