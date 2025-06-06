using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTemplateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Button _lobbyButton;

    private Lobby lobby;

    private void OnValidate()
    {
        if(_lobbyButton == null)
            _lobbyButton = GetComponent<Button>();
    }

    private void Awake()
    {
        _lobbyButton.onClick.AddListener(() => {
            ServiceLocator.Current.Get<LobbyRelayManager>().JoinWithId(lobby.Id);
        });
    }

    public void SetLobby(Lobby lobby)
    {
        this.lobby = lobby;
        UpdateLobbyData(lobby);
    }

    private void UpdateLobbyData(Lobby lobby)
    {
        lobbyNameText.text = lobby.Name;
        playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }
}
