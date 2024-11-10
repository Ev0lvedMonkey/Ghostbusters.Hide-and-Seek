using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private bool _isMenuOpen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isMenuOpen == false)
            {
                GameStateManager.Instance.OnOpenHUD.Invoke();
                CursorController.DisableCursor();
            }
            else
            {
                GameStateManager.Instance.OnCloseHUD.Invoke();
                CursorController.EnableCursor();
            }
            _isMenuOpen = !_isMenuOpen;
        }
    }
}
