using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSceneConfigurationScriptableObject", menuName = "GameSceneConfiguration")]
public class GameSceneConfiguration : ScriptableObject
{
    [SerializeField] private GameObject _level;
    [SerializeField] private List<GameObject> _enviromentType;

    public GameObject GetLevelObject()
    {
        return _level;
    }

    public GameObject GetEnviroment(int index)
    {
        Debug.Log(
            $"[GameSceneConfiguration] GetEnviroment {index} on player {ServiceLocator.Current.Get<MultiplayerStorage>().GetPlayerData().playerName}");
        return _enviromentType[index];
    }

    public int GetEnviromentRndIndex()
    {
        return Random.Range(0, _enviromentType.Count);
    }
}