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
            }
            else
            {
                GameStateManager.Instance.OnCloseHUD.Invoke();
            }
            _isMenuOpen = !_isMenuOpen;
        }
    }
}
