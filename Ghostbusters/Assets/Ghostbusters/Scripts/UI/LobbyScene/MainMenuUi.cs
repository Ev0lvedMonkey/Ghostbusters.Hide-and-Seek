using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _quickJoinLobbyBtn;
    [SerializeField] private Button _joinLobbyBtn;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private TMP_InputField _lobbyCodeInputField;
    [SerializeField] private TMP_InputField _playerNameInputField;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;
    [SerializeField] private CreateLobbyUI lobbyCreateUI;


    public void Init()
    {
        Dispose();
        UpdateLobbyList(new List<Lobby>());
        LobbyRelayManager.Instance.OnLobbyListChanged += OnLobbyListChanged;
        LobbyRelayManager.Instance.OnSignIn.AddListener(InitButton);
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void OnLobbyListChanged(object sender, LobbyRelayManager.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyTemplateUI>().SetLobby(lobby);
        }
    }

    private void InitButton()
    {
        ActivateButtons();
        _playerNameInputField.text = MultiplayerStorage.Instance.GetPlayerName();
        _playerNameInputField.onValueChanged.AddListener((string newText) => {
            MultiplayerStorage.Instance.SetPlayerName(newText);
        });
        _createLobbyBtn.onClick.AddListener(lobbyCreateUI.Show); 
        _joinLobbyBtn.onClick.AddListener(TestJoinWithCode);
        _quickJoinLobbyBtn.onClick.AddListener(TestQuickJoin);
        _menuBtn.onClick.AddListener(() => { SceneLoader.Load(SceneLoader.Scene.MenuScene);});
    }

    private void Dispose()
    {
        DeactivateButtons();
        _joinLobbyBtn.onClick.RemoveListener(TestJoinWithCode);
    }

    private void ActivateButtons()
    {
        _createLobbyBtn.gameObject.SetActive(true);
        _joinLobbyBtn.gameObject.SetActive(true);
        _quickJoinLobbyBtn.gameObject.SetActive(true);
        _lobbyCodeInputField.gameObject.SetActive(true);
    }

    private void DeactivateButtons()
    {
        _createLobbyBtn.gameObject.SetActive(false);
        _joinLobbyBtn.gameObject.SetActive(false);
        _quickJoinLobbyBtn.gameObject.SetActive(false);
        _lobbyCodeInputField.gameObject.SetActive(false);
        lobbyTemplate.gameObject.SetActive(false);
    }

    private async void TestQuickJoin()
    {
        await LobbyRelayManager.Instance.QuickJoin();
    }

    private async void TestJoinWithCode()
    {
        await LobbyRelayManager.Instance.JoinByCode(_lobbyCodeInputField.text);
    }
}
