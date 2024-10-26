using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameConnectionHandler : MonoBehaviour
{
    internal UnityEvent OnLoseConnnetion;
    internal UnityEvent OnConnneted;


    private const string CheckUrl = "http://google.com";
    private const float CheckInterval = 5f;
    private bool isGamePaused = false;

    private void Start()
    {
        StartCoroutine(CheckConnectionRoutine());
    }

    private IEnumerator CheckConnectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CheckInterval);
            yield return CheckInternetConnection(isConnected =>
            {
                if (isConnected && isGamePaused)
                {
                    ResumeGame();
                }
                else if (!isConnected && !isGamePaused)
                {
                    PauseGame();
                }
            });
        }
    }

    private IEnumerator CheckInternetConnection(System.Action<bool> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(CheckUrl))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();
            bool isConnected = request.result != UnityWebRequest.Result.ConnectionError &&
                               request.result != UnityWebRequest.Result.ProtocolError;
            onComplete(isConnected);
        }
    }

    private void PauseGame()
    {
        OnLoseConnnetion?.Invoke();
        Time.timeScale = 0; // Останавливает время в игре
        isGamePaused = true;
        Debug.Log("Game Paused: No internet connection.");
    }

    private void ResumeGame()
    {
        OnConnneted?.Invoke();
        Time.timeScale = 1; // Возобновляет время в игре
        isGamePaused = false;
        Debug.Log("Game Resumed: Internet connection restored.");
    }
}
