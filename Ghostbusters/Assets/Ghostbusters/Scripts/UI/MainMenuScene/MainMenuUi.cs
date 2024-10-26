using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _joinLobbyBtn;
    [SerializeField] private Button _quitBtn;
    [SerializeField] private TMP_InputField _lobbyCodeInputField;

    public void Init()
    {
        //_createLobbyBtn.onClick.AddListener(LobbyRelayManager.Instance.TestLobby);
        //_joinLobbyBtn.onClick.AddListener(JoinLobby);
        //_quitBtn.onClick.AddListener(Application.Quit);
    }
}
