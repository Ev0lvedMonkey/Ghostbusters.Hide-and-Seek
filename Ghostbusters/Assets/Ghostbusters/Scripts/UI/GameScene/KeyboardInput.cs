using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private bool _isMenuOpen;
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
            {
                _gameStateManager.OnOpenHUD.Invoke();
            }
            else
            {
                _gameStateManager.OnCloseHUD.Invoke();
            }
            _isMenuOpen = !_isMenuOpen;
        }
    }
}
