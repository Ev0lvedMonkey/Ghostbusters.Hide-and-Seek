using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum ScenesEnum
    {
        CharactersScene,
        MenuScene,
        GameScene,
        LoadingScene,
        LobbyScene
    }

    private static ScenesEnum _targetScene;

    public static void Load(ScenesEnum targetScene)
    {
        _targetScene = targetScene;
        SceneManager.LoadSceneAsync(ScenesEnum.LoadingScene.ToString());
    }

    public static ScenesEnum GetTargetScene()
    {
        return _targetScene;
    }

    public static void LoadNetwork(ScenesEnum targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback(MonoBehaviour caller)
    {
        caller.StartCoroutine(LoadSceneAsync(_targetScene));
    }

    private static IEnumerator LoadSceneAsync(ScenesEnum targetScene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene.ToString());
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}