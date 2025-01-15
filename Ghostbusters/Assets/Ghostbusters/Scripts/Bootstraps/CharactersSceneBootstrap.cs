using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactersSceneBootstrap : MonoBehaviour
{
    [SerializeField] private CharacterSelectReady _characterSelectReady;
    [SerializeField] private CharacterSelectUI _characterSelectUI;
    [SerializeField] private HostDisconnectUI _hostDisconnectUI;
    [SerializeField] private List<PlayerInfo> _listPlayerInfo;

    private void Awake()
    {
        _characterSelectUI.Init(_characterSelectReady);
    }

    private void OnEnable()
    {
        _characterSelectReady.MakeNewPlayerReadyDictionary();
        _characterSelectUI.SetLobbyData();
        _hostDisconnectUI.Init();
        InitPlayersData();
    }

    private void OnDisable()
    {
        _characterSelectUI.Uninit();
        _hostDisconnectUI.Uninit();
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
        foreach (var playerInfo in _listPlayerInfo)
        {
            if (playerInfo.TryGetComponent(out ClientPlayerInfo clientInfo))
                clientInfo.Init(_characterSelectReady);
            playerInfo.Init(_characterSelectReady);
        }
    }

    private void UnnitPlayersData()
    {
        if (_listPlayerInfo == null || _listPlayerInfo.Count == 0)
            return;
        foreach (var item in _listPlayerInfo)
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

        var duplicates = _listPlayerInfo
            .GroupBy(player => player)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicates.Any())
            Debug.LogError($"Duplicate PlayerInfo objects found: {string.Join(", ", duplicates)}");
    }
}
