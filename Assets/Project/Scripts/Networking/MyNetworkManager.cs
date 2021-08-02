using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Open.Nat;
using System.Threading;

public class MyNetworkManager : NetworkManager {
    [Header("Objects")]
    [SerializeField] private GameObject lobbyPlayerPrefab;
    [SerializeField] private GameObject gamePlayerPrefab;

    [Header("Stats")]
    [SerializeField] private int minPlayers;
    [SerializeField] private int maxPlayers;

    private int _UPnPPort = 7777;
    private string _UPnPDescription = "Mapping created by Slap";

    public override async void Start() {
        NatDiscoverer discoverer = new NatDiscoverer();
        CancellationTokenSource cts = new CancellationTokenSource(10000);
        NatDevice device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, _UPnPPort, _UPnPPort, _UPnPDescription));

        base.Start();
    }

    public override void OnServerReady(NetworkConnection conn) {
        base.OnServerReady(conn);

        if (SceneManager.GetActiveScene().name == GameManager.LOBBY_SCENE_NAME) {
            GameObject lobbyPlayerObject = Instantiate(lobbyPlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, lobbyPlayerObject);
        } else if (SceneManager.GetActiveScene().name.Substring(0, 5) == "Level") { // If it's a level
            Transform startPos = GetStartPosition();

            GameObject gamePlayerObject = Instantiate(gamePlayerPrefab, startPos.position, startPos.rotation);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerObject);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn) {
        GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab);

        NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
    }
}