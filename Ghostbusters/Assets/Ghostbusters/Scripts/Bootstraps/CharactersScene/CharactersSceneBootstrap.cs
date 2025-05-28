using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class CharactersSceneBootstrap : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private CharactersSceneUIBootstrap _charactersSceneUIBootstrap;
    [FormerlySerializedAs("_characterSelectReady")] [SerializeField] private PlayerReadyManager playerReadyManager;
    [SerializeField] private List<PlayerInfo> _listPlayerInfo;

    private void OnEnable()
    {
        _charactersSceneUIBootstrap.Init(playerReadyManager);
        InitPlayersData();
    }

    private void OnDisable()
    {
        _charactersSceneUIBootstrap.Uninit();
        UnnitPlayersData();
    }

    private void OnValidate()
    {
        ValidatePlayerInfoList();
    }

    private void InitPlayersData()
    {
        if (_listPlayerInfo == null || _listPlayerInfo.Count == 0)
            return;
        foreach (PlayerInfo playerInfo in _listPlayerInfo)
        {
            if (playerInfo.TryGetComponent(out ClientPlayerInfo clientInfo))
            {
                clientInfo.Init(playerReadyManager);
                continue;
            }
            playerInfo.Init(playerReadyManager);
        }
    }

    private void UnnitPlayersData()
    {
        if (_listPlayerInfo == null || _listPlayerInfo.Count == 0)
            return;
        foreach (PlayerInfo item in _listPlayerInfo)
        {
            if (item.TryGetComponent(out ClientPlayerInfo component))
                component.Uninit();
            item.Uninit();
        }
    }

    private void ValidatePlayerInfoList()
    {
        if (_listPlayerInfo == null || _listPlayerInfo.Count == 0)
            return;

        List<PlayerInfo> duplicates = _listPlayerInfo
            .GroupBy(player => player)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicates.Any())
            Debug.LogError($"Duplicate PlayerInfo objects found: {string.Join(", ", duplicates)}");
    }
}