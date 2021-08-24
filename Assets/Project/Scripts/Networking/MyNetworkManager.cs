using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Open.Nat;
using System.Threading;

public class MyNetworkManager : NetworkManager {
    public static MyNetworkManager Instance;

    [Header("Objects")]
    [SerializeField] private GameObject lobbyPlayerPrefab;
    public GameObject GamePlayerPrefab;

    public string asdf;

    [Header("Stats")]
    [SerializeField] private bool searchForUPNP;
    [SerializeField] private int minPlayers;
    [SerializeField] private int maxPlayers;

    private int _UPnPPort = 7777;
    private string _UPnPDescription = "Mapping created by Slap";

    private int _playersLoaded = 0;

    public override void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        base.Awake();
    }

    public override async void Start() {
        if (searchForUPNP) {
            NatDiscoverer discoverer = new NatDiscoverer();
            CancellationTokenSource cts = new CancellationTokenSource(10000);
            NatDevice device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

            await device.CreatePortMapAsync(new Mapping(Protocol.Udp, _UPnPPort, _UPnPPort, _UPnPDescription));
        }

        base.Start();
    }

    public override void OnServerReady(NetworkConnection conn) {
        base.OnServerReady(conn);

        if (SceneManager.GetActiveScene().name == GameManager.Instance.LOBBY_SCENE_NAME) {
            GameObject lobbyPlayerObject = Instantiate(lobbyPlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, lobbyPlayerObject);
        } else if (SceneManager.GetActiveScene().name.Substring(0, 5) == "Level") { // If it's a level
            Transform startPos = GetStartPosition();

            GameObject gamePlayerObject = Instantiate(GamePlayerPrefab, startPos.position, startPos.rotation);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerObject);

            _playersLoaded++;

            if (_playersLoaded == NetworkServer.connections.Count) {
                _playersLoaded = 0; // Reset for next level

                LevelManager levelManager = GameObject.Find("Level").GetComponent<LevelManager>();

                levelManager.AllPlayersLoaded();
            }
        }
    }
}