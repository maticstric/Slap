using UnityEngine;
using Mirror;

public class PlayerDeath : NetworkBehaviour {
    private Player _player;

    private void Awake() {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.name == "DeathTrigger") {
            if (isLocalPlayer) {
                _player.CmdSetIsAlive(false);
            }
        }
    }

    [Server]
    public void SwitchLevel() {
        string randomLevel = GameManager.Instance.GetRandomLevel();
        MyNetworkManager.singleton.ServerChangeScene(randomLevel);
    }

    public int CountAlivePlayers() {
        GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int count = 0;

        foreach (GameObject playerGameObject in playerGameObjects) {
            Player player = playerGameObject.GetComponent<Player>();

            if (player.IsAlive) {
                count++;
            }
        }

        return count;
    }
}
