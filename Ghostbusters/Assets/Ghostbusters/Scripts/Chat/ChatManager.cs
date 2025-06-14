using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ChatManager : NetworkBehaviour, IService
{
    private readonly Queue<ChatMessage> _chatMessages = new();
    private const int MaxMessages = 20;
    private const int MaxMessageLength = 100;

    [Header("Components")] [SerializeField]
    private ChatMessage _chatMessagePrefab;

    [SerializeField] private CanvasGroup _chatContent;
    [SerializeField] private TMP_InputField _chatInput;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _scrollRectBody;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private EventSystem _eventSystem;

    private PlayerSessionManager _playerSessionManager;
    private GameStateManager _gameStateManager;
    private GameOverWinUI _overWinUI;
    private bool _isChatOpen;
    private bool _isOwnerDead;

    internal UnityEvent OnChatStateChanged = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && _chatInput.text.Length <= MaxMessageLength)
        {
            SendChatMessage(_chatInput.text, _playerSessionManager.GetPlayerName(),
                _playerSessionManager.GetLocalPlayerID());
            _chatInput.text = string.Empty;
        }
    }

    private void StateHandler()
    {
        OnChatStateChanged.Invoke();
        if (_isOwnerDead || _overWinUI.IsOpened())
        {
            return;
        }
        _isChatOpen = !_isChatOpen;

        _scrollRectBody.SetActive(_isChatOpen);
        _chatInput.gameObject.SetActive(_isChatOpen);
        if (_isChatOpen)
            StartCoroutine(ScrollToBottom());
    }

    public void Init()
    {
        _playerSessionManager = ServiceLocator.Current.Get<PlayerSessionManager>();
        _gameStateManager = ServiceLocator.Current.Get<GameStateManager>();
        _gameStateManager.OnChatClose.AddListener(StateHandler);
        _gameStateManager.OnChatOpen.AddListener(StateHandler);
        _overWinUI = ServiceLocator.Current.Get<GameOverWinUI>();
        _scrollRectBody.SetActive(false);
        _chatInput.gameObject.SetActive(false);
        _chatInput.characterLimit = MaxMessageLength;
    }

    public void SetOwnerDead()
    {
        _isOwnerDead = true;
        _body.SetActive(false);
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

        if (_playerSessionManager.GetLocalPlayerID() == ownerId)
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