using UnityEngine;
using Mirror;
using TMPro;

public class LobbyPlayer : NetworkBehaviour {
    private const string LOBBY_OBJECT_NAME = "Lobby";
    private const string LOBBY_PLAYERS_OBJECT_NAME = "LobbyPlayers";
    private const string READY_STRING = "<color=green>Ready</color>";
    private const string NOT_READY_STRING = "<color=red>Not Ready</color>";

    private Transform _canvas;
    private Transform _lobby;
    private Transform _lobbyPlayers;
    private LobbyManager _lobbyManager;

    [Header("Stats")]
    [SyncVar(hook = nameof(OnUsernameChanged))]
    public string Username;
    [SyncVar(hook = nameof(OnIsReadyChanged))]
    public bool IsReady;

    [Header("Objects")]
    public TextMeshProUGUI UsernameText;
    public TextMeshProUGUI ReadyText;

    private void Awake() {
        _canvas = FindObjectOfType<Canvas>().transform;
        _lobby = _canvas.Find(LOBBY_OBJECT_NAME);
        _lobbyPlayers = _lobby.Find(LOBBY_PLAYERS_OBJECT_NAME);
        _lobbyManager = FindObjectOfType<LobbyManager>();
    }

    private void Start() {
        transform.SetParent(_lobbyPlayers);
        transform.localPosition = Vector3.zero;
    }

    public override void OnStartAuthority() {
        CmdSetUsername(GameManager.Instance.Username);
        CmdSetIsReady(false);
    }

    [Command]
    private void CmdSetUsername(string username) {
        Username = username;
    }

    [Command]
    public void CmdSetIsReady(bool isReady) {
        IsReady = isReady;
    }

    private void OnUsernameChanged(string oldValue, string newValue) {
        UsernameText.text = newValue;
    }

    private void OnIsReadyChanged(bool oldValue, bool newValue) {
        ReadyText.text = newValue ? READY_STRING : NOT_READY_STRING;

        if (isServer) {
            if (_lobbyManager.AllPlayersReady()) {
                _lobbyManager.SetStartButtonActive(true);
            } else {
                _lobbyManager.SetStartButtonActive(false);
            }
        }
    }
}