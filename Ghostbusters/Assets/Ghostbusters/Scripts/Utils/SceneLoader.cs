using System.Diagnostics;
using Unity.Netcode;
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

        SceneManager.LoadScene(ScenesEnum.LoadingScene.ToString());
    }

    public static ScenesEnum GetTargetScene()
    {
        return _targetScene;
    }

    public static void LoadNetwork(ScenesEnum targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(_targetScene.ToString());
    }

}