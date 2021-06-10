using UnityEngine;
using Mirror;
using TMPro;

public class LobbyManager : MonoBehaviour {
    private const string READY_STRING = "Ready";
    private const string NOT_READY_STRING = "Cancel";

    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI readyButtonText;
    [SerializeField] private GameObject startButton;

    public void ToggleIsReady() {
        LobbyPlayer localLobbyPlayer = NetworkClient.localPlayer.GetComponent<LobbyPlayer>();

        bool isReady = localLobbyPlayer.IsReady;
        isReady = !isReady;

        readyButtonText.text = isReady ? NOT_READY_STRING : READY_STRING;
        localLobbyPlayer.CmdSetIsReady(isReady);
    }

    [Server]
    public void StartGame() {
        MyNetworkManager.singleton.ServerChangeScene(GameManager.GAME_SCENE_NAME);
    }

    [Server]
    public bool AllPlayersReady() {
        print(NetworkServer.connections.Values.Count);
        foreach (NetworkConnection conn in NetworkServer.connections.Values) {
            LobbyPlayer lobbyPlayer = conn.identity.GetComponent<LobbyPlayer>();

            if (!lobbyPlayer.IsReady) {
                return false;
            }
        }

        return true;
    }

    [Server]
    public void SetStartButtonActive(bool isActive) {
        startButton.SetActive(isActive);
    }
}