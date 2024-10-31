using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _joinLobbyBtn;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private TMP_InputField _lobbyCodeInputField;

    public void Init()
    {
        LobbyRelayManager.Instance.OnSignIn.AddListener(InitButton);
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void InitButton()
    {
        ActivateButtons();
        _createLobbyBtn.onClick.AddListener(TestCreateLobby);
        _joinLobbyBtn.onClick.AddListener(TestJoinWithCode);
        _menuBtn.onClick.AddListener(() => { SceneLoader.Load(SceneLoader.Scene.MenuScene);});
    }

    private void Dispose()
    {
        DeactivateButtons();
        _createLobbyBtn.onClick.RemoveListener(TestCreateLobby);
        _joinLobbyBtn.onClick.RemoveListener(TestJoinWithCode);
    }

    private void ActivateButtons()
    {
        _createLobbyBtn.interactable = true;
        _joinLobbyBtn.interactable = true;
    }

    private void DeactivateButtons()
    {
        _createLobbyBtn.interactable = false;
        _joinLobbyBtn.interactable = false;
    }

    private async void TestJoinWithCode()
    {
        await LobbyRelayManager.Instance.JoinByCode(_lobbyCodeInputField.text);
    }

    private async void TestCreateLobby()
    {
        await LobbyRelayManager.Instance.CreateLobby("Lobby_" + Random.Range(1, 200));
    }
}
