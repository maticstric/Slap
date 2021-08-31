using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Open.Nat;
using System.Threading;
using System.Collections.Generic;

public class MyNetworkManager : NetworkManager {
    public static MyNetworkManager Instance;

    [Header("Objects")]
    [SerializeField] private GameObject lobbyPlayerPrefab;
    public GameObject GamePlayerPrefab;
    public Texture GamePlayerTexture;

    [Header("Stats")]
    [SerializeField] private bool searchForUPNP;
    [SerializeField] private int minPlayers;
    [SerializeField] private int maxPlayers;

    private int _UPnPPort = 7777;
    private string _UPnPDescription = "Mapping created by Slap";

    private int _playersLoaded = 0;

    public Dictionary<int, CharacterSelect.Characters> Characters;
    public Dictionary<int, CharacterSelect.Colors> Colors;

    public override void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        Characters = new Dictionary<int, CharacterSelect.Characters>();
        Colors = new Dictionary<int, CharacterSelect.Colors>();

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

            GameObject gamePlayerPrefab = CharacterSelect.Instance.GetCharacterPrefab(Characters[conn.connectionId]);
            GameObject gamePlayerObject = Instantiate(gamePlayerPrefab, startPos.position, startPos.rotation);

            gamePlayerObject.GetComponent<Player>().Character = Characters[conn.connectionId];
            gamePlayerObject.GetComponent<Player>().Color = (int)Colors[conn.connectionId];

            NetworkServer.AddPlayerForConnection(conn, gamePlayerObject);

            _playersLoaded++;

            if (_playersLoaded == NetworkServer.connections.Count) {
                _playersLoaded = 0; // Reset for next level

                LevelManager levelManager = GameObject.Find("Level").GetComponent<LevelManager>();

                levelManager.AllPlayersLoaded();
            }
        }
    }

    public void SetCharacter(CharacterSelect.Characters character, NetworkConnectionToClient sender) {
        Characters[sender.connectionId] = character;
    }

    public void SetColor(CharacterSelect.Colors color, NetworkConnectionToClient sender) {
        Colors[sender.connectionId] = color;
    }
}