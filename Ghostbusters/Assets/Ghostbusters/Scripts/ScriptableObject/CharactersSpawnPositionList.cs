using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPositionListScriptableObject", menuName = "SpawnPositionList")]
public class CharactersSpawnPositionList : ScriptableObject
{
    [SerializeField] private List<Vector3> _spawnPositionList;

   public Vector3 GetSpawnPositon(int index)
    {
        return _spawnPositionList[index];
    }
}
