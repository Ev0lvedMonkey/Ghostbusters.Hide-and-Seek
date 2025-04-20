using UnityEngine;

public class MenuSceneBootstrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MenuUIBootstrap _menuUIBootstrap;
    [SerializeField] private SingltonsCleanUp _singltonCleanUp;
    [SerializeField] private CamRotateAround _camRotateAround;
    [SerializeField] private AudioManager _audioManager;

    private void Awake()
    {
        _singltonCleanUp.CleanUp();
        ServiceLocator.Inizialize();
        ServiceLocator.Current.Register(_audioManager);
        
        _audioManager.Init();
        _camRotateAround.DefinitionOfRotateDirection();
        _camRotateAround.Init();
        CursorController.EnableCursor();
        _menuUIBootstrap.Init();
    }

    private void OnDisable()
    {
        _menuUIBootstrap.Uninit();
    }
}
