using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, IService
{
    [SerializeField] private AudioSource _audioSource;

    private void OnValidate()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name == SceneLoader.ScenesEnum.MenuScene.ToString())
            _audioSource.Play();
        if(SceneManager.GetActiveScene().name == SceneLoader.ScenesEnum.GameScene.ToString())
            _audioSource.Stop();
    }

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
    }
}
