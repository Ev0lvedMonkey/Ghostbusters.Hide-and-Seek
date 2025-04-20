using UnityEngine;

public class CharactersSceneUIBootstrap : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CharacterSelectUI _characterSelectUI;
    [SerializeField] private HostDisconnectUI _hostDisconnectUI;
    
    public void Init( CharacterSelectReady characterSelectReady)
    {
        _characterSelectUI.Init(characterSelectReady);
        _characterSelectUI.UpdateLobbyData();
        _hostDisconnectUI.Init();
    }

    public void Uninit()
    {
        _characterSelectUI.Uninit();
        _hostDisconnectUI.Uninit();
    }
}