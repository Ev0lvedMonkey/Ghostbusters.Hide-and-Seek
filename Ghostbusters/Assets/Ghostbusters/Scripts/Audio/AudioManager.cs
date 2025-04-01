using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, IService
{
    [SerializeField] private AudioSource _audioSource;

    private void OnValidate()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        _audioSource.mute = !hasFocus;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        _audioSource.mute = pauseStatus;
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
