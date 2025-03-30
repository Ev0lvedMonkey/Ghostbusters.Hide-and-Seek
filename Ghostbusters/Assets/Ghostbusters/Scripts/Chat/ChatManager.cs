using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatManager : NetworkBehaviour, IService
{
    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] TMP_InputField chatInput;
    [SerializeField] GameObject body;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] EventSystem eventSystem;

    private MultiplayerStorage _multiplayerStorage;
    private bool _isChatOpen;
    private Queue<ChatMessage> _chatMessages = new();
    private const int maxMessages = 20;
    private const int maxMessageLength = 100;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && chatInput.text.Length <= maxMessageLength)
        {
            SendChatMessage(chatInput.text, _multiplayerStorage.GetPlayerName(), _multiplayerStorage.GetLocalPlayerID());
            chatInput.text = string.Empty;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isChatOpen = !_isChatOpen;
            body.SetActive(_isChatOpen);
            if (_isChatOpen)
                StartCoroutine(ScrollToBottom());
        }
    }
    public void Init()
    {
        _multiplayerStorage = ServiceLocator.Current.Get<MultiplayerStorage>();
        body.SetActive(false);
        chatInput.characterLimit = maxMessageLength;
    }

    public bool IsOpened()
    {
        return _isChatOpen;
    }

    public void SendChatMessage(string message, string messageOwner, ulong ownerId)
    {
        if (string.IsNullOrWhiteSpace(message) || message.Length > maxMessageLength) return;
        SendChatMessageServerRpc(message, messageOwner, ownerId);
    }

    private void AddMessage(string message, string messageOwner, ulong ownerId)
    {
        chatInput.text = string.Empty;
        if (_chatMessages.Count > maxMessages)
        {
            Destroy(_chatMessages.Dequeue().gameObject);
        }

        if (_multiplayerStorage.GetLocalPlayerID() == ownerId)
        {
            messageOwner = $"<color=yellow>{messageOwner}</color>";
        }

        ChatMessage chatMessage = Instantiate(chatMessagePrefab, chatContent.transform);
        chatMessage.SetText($"{messageOwner}: {message}");
        _chatMessages.Enqueue(chatMessage);





        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0f;
        eventSystem.SetSelectedGameObject(chatInput.gameObject);
        chatInput.ActivateInputField();
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
