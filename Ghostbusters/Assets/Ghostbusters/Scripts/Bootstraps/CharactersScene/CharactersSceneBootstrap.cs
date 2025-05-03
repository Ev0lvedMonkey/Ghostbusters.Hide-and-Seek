using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CharactersSceneBootstrap : MonoBehaviour
{
    [Header("Сomponents")] [SerializeField]
    private CharactersSceneUIBootstrap _charactersSceneUIBootstrap;

    [SerializeField] private CharacterSelectReady _characterSelectReady;
    [SerializeField] private List<PlayerInfo> _listPlayerInfo;

    private void OnEnable()
    {
        _charactersSceneUIBootstrap.Init(_characterSelectReady);
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
                clientInfo.Init(_characterSelectReady);
                continue;
            }
            playerInfo.Init(_characterSelectReady);
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