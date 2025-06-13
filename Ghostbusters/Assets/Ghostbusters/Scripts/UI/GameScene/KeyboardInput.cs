using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private bool _isMenuOpen;
    private bool _isChatOpen;
    private GameStateManager _gameStateManager;

    public void Init(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isMenuOpen == false)
                _gameStateManager.OnOpenHUD.Invoke();
            else
                _gameStateManager.OnCloseHUD.Invoke();
            _isMenuOpen = !_isMenuOpen;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isChatOpen == false)
            {
                _gameStateManager.OnChatOpen.Invoke();
                CursorController.EnableCursor();
            }
            else
            {
                _gameStateManager.OnChatClose.Invoke();
                CursorController.DisableCursor();
            }
            _isChatOpen = !_isChatOpen;
        }
    }
}
