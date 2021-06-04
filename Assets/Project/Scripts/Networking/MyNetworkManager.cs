using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkRoomManager {
    public static MyNetworkManager Instance;

    public override void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }
    }

    //public override void OnClientConnect(NetworkConnection conn) {
    //    NetworkClient.Ready();
    //}
}