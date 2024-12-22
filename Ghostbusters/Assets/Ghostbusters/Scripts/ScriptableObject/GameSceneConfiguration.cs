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

    public GameObject GetEnviroment()
    {
        int randomIndex = Random.Range(0, _enviromentType.Count);
        Debug.Log($"randomIndex {randomIndex}");
        return _enviromentType[randomIndex];
    }

    public GameObject GetEnviroment(int index)
    {
        return _enviromentType[index];
    }
}
