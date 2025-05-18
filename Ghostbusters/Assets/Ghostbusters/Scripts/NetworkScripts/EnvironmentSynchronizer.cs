using Unity.Netcode;
using UnityEngine;

public class EnvironmentSynchronizer : NetworkBehaviour
{
    [SerializeField] private GameSceneConfiguration _configuration;

    private NetworkVariable<int> _environmentIndex =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private EnvironmentSpawner _spawner;
    private GameObject _spawnedEnvironment;

    public override void OnNetworkSpawn()
    {
        _environmentIndex.OnValueChanged += OnEnvironmentIndexChanged;
        if (IsHost)
        {
            _environmentIndex.Value = _configuration.GetEnviromentRndIndex();
            LoadEnvironment(_environmentIndex.Value);
        }
        Debug.Log($"[EnvironmentSynchronizer] OnNetworkSpawn {_environmentIndex.Value}");
        
        if(_environmentIndex.Value > 0)
            LoadEnvironment(_environmentIndex.Value);
    }

    private void OnEnvironmentIndexChanged(int oldIndex, int newIndex)
    {
        LoadEnvironment(newIndex);
    }

    private void LoadEnvironment(int index)
    {
        if (_spawnedEnvironment != null)
        {
            Destroy(_spawnedEnvironment);
        }

        _spawnedEnvironment = Instantiate(_configuration.GetEnviroment(index));
        _spawner = _spawnedEnvironment.GetComponent<EnvironmentSpawner>();
        if (IsHost)
            _spawner.SpawnAllHost();
        else
            _spawner.SpawnAllClient();

        Debug.Log($"[EnvironmentSynchronizer] Loaded environment index: {index}");
    }
}