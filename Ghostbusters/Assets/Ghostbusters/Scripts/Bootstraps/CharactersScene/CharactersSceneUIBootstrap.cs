using UnityEngine;

public class CharactersSceneUIBootstrap : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CharacterSelectUI _characterSelectUI;
    [SerializeField] private HostDisconnectUI _hostDisconnectUI;
    
    public void Init( PlayerReadyManager playerReadyManager)
    {
        _characterSelectUI.Init(playerReadyManager);
        _characterSelectUI.UpdateLobbyData();
        _hostDisconnectUI.Init();
    }

    public void Uninit()
    {
        _characterSelectUI.Uninit();
        _hostDisconnectUI.Uninit();
    }
}