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
                CursorController.EnableCursor();
            }
            else
            {
                GameStateManager.Instance.OnCloseHUD.Invoke();
                CursorController.DisableCursor();
            }
            _isMenuOpen = !_isMenuOpen;
        }
    }
}
