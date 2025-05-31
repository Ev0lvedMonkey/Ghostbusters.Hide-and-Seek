using UnityEngine;

public class ChatIconView : MonoBehaviour
{
    private ChatManager _chatManager;

    private void Start()
    {
        _chatManager = ServiceLocator.Current.Get<ChatManager>();
        _chatManager.OnChatStateChanged.AddListener(HandleChatStateChange);
    }

    private void HandleChatStateChange()
    {
        gameObject.SetActive(_chatManager.IsOpened());
    }
}