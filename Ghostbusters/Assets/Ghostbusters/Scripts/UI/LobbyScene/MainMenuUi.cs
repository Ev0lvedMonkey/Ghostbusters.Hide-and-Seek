using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _quickJoinLobbyBtn;
    [SerializeField] private Button _joinLobbyByCodeBtn;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private TMP_InputField _lobbyCodeInputField;
    [SerializeField] private TMP_InputField _playerNameInputField;
    [SerializeField] private Transform _lobbyContainer;
    [SerializeField] private Transform _lobbyTemplate;
    [SerializeField] private CreateLobbyUI _lobbyCreateUI;
    
    private ServiceLocator _serviceLocator;
    private LobbyRelayManager _lobbyRelayManager;
    
    public void Init()
    {
        _serviceLocator = null;
        _lobbyRelayManager = null;
        _serviceLocator = ServiceLocator.Current;
        _lobbyRelayManager = _serviceLocator.Get<LobbyRelayManager>();
        _playerNameInputField.characterLimit = 19;
        _lobbyCodeInputField.characterLimit = 6;
        DeactivateButtons();
        UpdateLobbyList(new List<Lobby>());
        _lobbyRelayManager.OnLobbyListChanged += OnLobbyListChanged;
        _lobbyRelayManager.OnSignIn.AddListener(ActivateButtons);
        _lobbyRelayManager.OnSignIn.AddListener(AddButtonsListeners);
        AddButtonsListeners();
        Show();
        ActivateButtons();
    }

    public void Uninit()
    {
        _lobbyRelayManager.OnLobbyListChanged -= OnLobbyListChanged;
        _lobbyRelayManager.OnSignIn.RemoveListener(ActivateButtons);
        _lobbyRelayManager.OnSignIn.RemoveListener(AddButtonsListeners);
        RemoveButtonsListeners();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void OnLobbyListChanged(object sender, LobbyRelayManager.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in _lobbyContainer)
        {
            if (child == _lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyTemplateUI>().SetLobby(lobby);
        }
    }

    private void AddButtonsListeners()
    {
        _playerNameInputField.text = _serviceLocator.Get<MultiplayerStorage>().GetPlayerName();
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            _playerNameInputField.onValueChanged.AddListener((string newText) =>
               _serviceLocator.Get<MultiplayerStorage>().SetPlayerName(newText));
            _createLobbyBtn.onClick.AddListener(_lobbyCreateUI.Show);
            _joinLobbyByCodeBtn.onClick.AddListener(JoinWithCode);
            _quickJoinLobbyBtn.onClick.AddListener(QuickJoin);
            _menuBtn.onClick.AddListener(() => SceneLoader.Load(SceneLoader.ScenesEnum.MenuScene));
        }
    }

    private void RemoveButtonsListeners()
    {
        _playerNameInputField.onValueChanged.RemoveListener((string newText) =>
           _serviceLocator.Get<MultiplayerStorage>().SetPlayerName(newText));
        _createLobbyBtn.onClick.RemoveListener(_lobbyCreateUI.Show);
        _joinLobbyByCodeBtn.onClick.RemoveListener(JoinWithCode);
        _quickJoinLobbyBtn.onClick.RemoveListener(QuickJoin);
        _menuBtn.onClick.RemoveListener(() => SceneLoader.Load(SceneLoader.ScenesEnum.MenuScene));
    }

    private void ActivateButtons()
    { 
        if (AuthenticationService.Instance.IsSignedIn)
        {
            _createLobbyBtn.gameObject.SetActive(true);
            _joinLobbyByCodeBtn.gameObject.SetActive(true);
            _quickJoinLobbyBtn.gameObject.SetActive(true);
            _lobbyCodeInputField.gameObject.SetActive(true);
        }
    }

    private void DeactivateButtons()
    {
        _createLobbyBtn.gameObject.SetActive(false);
        _joinLobbyByCodeBtn.gameObject.SetActive(false);
        _quickJoinLobbyBtn.gameObject.SetActive(false);
        _lobbyCodeInputField.gameObject.SetActive(false);
        _lobbyTemplate.gameObject.SetActive(false);
    }

    private async void QuickJoin()
    {
        await _serviceLocator.Get<LobbyRelayManager>().QuickJoin();
    }

    private async void JoinWithCode()
    {
        string lobbyCode = _lobbyCodeInputField.text;
        if (string.IsNullOrWhiteSpace(lobbyCode))
            return;
        await _serviceLocator.Get<LobbyRelayManager>().JoinByCode(lobbyCode);
    }
}
