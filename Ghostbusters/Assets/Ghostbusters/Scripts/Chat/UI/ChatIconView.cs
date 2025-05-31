using UnityEngine;

public class ChatIconView : MonoBehaviour
{
    private ChatManager _chatManager;
    private GameOverWinUI _overWinUI;

    private void Start()
    {
        _chatManager = ServiceLocator.Current.Get<ChatManager>();
        _overWinUI = ServiceLocator.Current.Get<GameOverWinUI>();
        _chatManager.OnChatStateChanged.AddListener(HandleChatStateChange);
    }

    private void HandleChatStateChange()
    {
        if(_overWinUI.IsOpened())
            return;
        gameObject.SetActive(_chatManager.IsOpened());
    }
}