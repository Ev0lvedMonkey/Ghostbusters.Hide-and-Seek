using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ChatManager : NetworkBehaviour, IService
{
    [Header("Сomponents")]
    [SerializeField] private ChatMessage _chatMessagePrefab;
    [SerializeField] private CanvasGroup _chatContent;
    [SerializeField] private TMP_InputField _chatInput;
    [SerializeField] private GameObject _body;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private EventSystem _eventSystem;

    private MultiplayerStorage _multiplayerStorage;
    private bool _isChatOpen;
    private readonly Queue<ChatMessage> _chatMessages = new();
    
    private const int MaxMessages = 20;
    private const int MaxMessageLength = 100;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && _chatInput.text.Length <= MaxMessageLength)
        {
            SendChatMessage(_chatInput.text, _multiplayerStorage.GetPlayerName(), _multiplayerStorage.GetLocalPlayerID());
            _chatInput.text = string.Empty;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isChatOpen = !_isChatOpen;
            _body.SetActive(_isChatOpen);
            if (_isChatOpen)
                StartCoroutine(ScrollToBottom());
        }
    }
    public void Init()
    {
        _multiplayerStorage = ServiceLocator.Current.Get<MultiplayerStorage>();
        _body.SetActive(false);
        _chatInput.characterLimit = MaxMessageLength;
    }

    public bool IsOpened()
    {
        return _isChatOpen;
    }

    public void SendChatMessage(string message, string messageOwner, ulong ownerId)
    {
        if (string.IsNullOrWhiteSpace(message) || message.Length > MaxMessageLength) return;
        SendChatMessageServerRpc(message, messageOwner, ownerId);
    }

    private void AddMessage(string message, string messageOwner, ulong ownerId)
    {
        _chatInput.text = string.Empty;
        if (_chatMessages.Count > MaxMessages)
        {
            Destroy(_chatMessages.Dequeue().gameObject);
        }

        if (_multiplayerStorage.GetLocalPlayerID() == ownerId)
        {
            messageOwner = $"<color=yellow>{messageOwner}</color>";
        }

        ChatMessage chatMessage = Instantiate(_chatMessagePrefab, _chatContent.transform);
        chatMessage.SetText($"{messageOwner}: {message}");
        _chatMessages.Enqueue(chatMessage);
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        _scrollRect.verticalNormalizedPosition = 0f;
        _eventSystem.SetSelectedGameObject(_chatInput.gameObject);
        _chatInput.ActivateInputField();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message, string messageOwner, ulong ownerId)
    {
        ReceiveChatMessageClientRpc(message, messageOwner, ownerId);
    }

    [ClientRpc]
    private void ReceiveChatMessageClientRpc(string message, string messageOwner, ulong ownerId)
    {
        AddMessage(message, messageOwner, ownerId);
    }
}
