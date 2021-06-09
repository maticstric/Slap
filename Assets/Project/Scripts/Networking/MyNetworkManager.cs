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

        print(await device.GetExternalIPAsync());

        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, _UPnPPort, _UPnPPort, _UPnPDescription));
    }

    public override void OnServerReady(NetworkConnection conn) {
        if (SceneManager.GetActiveScene().name == GameManager.LOBBY_SCENE_NAME) {
            GameObject lobbyPlayerObject = Instantiate(lobbyPlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, lobbyPlayerObject);
        } else if (SceneManager.GetActiveScene().name == GameManager.GAME_SCENE_NAME) {
            GameObject gamePlayerObject = Instantiate(gamePlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerObject);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn) {
        GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab);

        NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
    }
}