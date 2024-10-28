using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _joinLobbyBtn;
    [SerializeField] private TMP_InputField _lobbyCodeInputField;

    private void OnEnable()
    {
        StartCoroutine(WaitForInit());
    }

    private IEnumerator WaitForInit()
    {
        yield return new WaitForSeconds(5);
        Init();
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void Init()
    {
        ActivateButtons();
        _createLobbyBtn.onClick.AddListener(TestCreateLobby);
        _joinLobbyBtn.onClick.AddListener(TestJoinWithCode);
        Debug.LogWarning("AddListeners");
    }

    private void Dispose()
    {
        DeactivateButtons();
        _createLobbyBtn.onClick.RemoveListener(TestCreateLobby);
        _joinLobbyBtn.onClick.RemoveListener(TestJoinWithCode);
        Debug.LogWarning("RemoveListeners");
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
        Debug.LogWarning("Click code");
        await LobbyRelayManager.Instance.JoinByCode(_lobbyCodeInputField.text);
    }
    private async void TestCreateLobby()
    {
        Debug.LogWarning("Click lobby");
        await LobbyRelayManager.Instance.CreateLobby("Lobby_" + Random.Range(1, 200));
    }

}
