using Zenject;
using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : NetworkManager
{
    //[Inject] private WeponFireFactory _weaponFireFactory;
    //[Inject] private TransformingShootFactory _transformingShootFactory;

    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    //{
    //    //SpawnNewPlayer(conn, _weaponFireFactory);
    //    //SpawnNewPlayer(conn, _transformingShootFactory);
    //}

    //private void SpawnNewPlayer<T>(NetworkConnectionToClient conn, IFactory<T> factory) where T : NetworkBehaviour
    //{
    //    var entity = factory.Create();
    //    Debug.Log($"{factory.GetType().Name} factory working");

    //    NetworkServer.AddPlayerForConnection(conn, entity.gameObject);
    //    NetworkServer.Spawn(entity.gameObject, conn);
    //}
}
