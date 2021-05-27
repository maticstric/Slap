using Unity;
using Mirror;

public class MyNetworkManager : NetworkManager {
    public override void OnStartServer() {
        print("StartServer");
    }

    public override void OnStopServer() {
        print("StopServer");
    }

    public override void OnClientConnect(NetworkConnection conn) {
        print("ClientConnect");
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        print("ClientDisconnect");
    }
}