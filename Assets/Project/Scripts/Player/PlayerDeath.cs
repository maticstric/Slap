using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class PlayerDeath : NetworkBehaviour {
    private Player _player;

    private void Awake() {
        _player = GetComponent<Player>();
    }

    private void OnTriggerExit(Collider collider) {
        if (isLocalPlayer) {
            if (collider.gameObject.name.Equals("DeathTrigger")) {
                _player.CmdSetIsAlive(false);

                List<Player> alivePlayers = GetAlivePlayers();

                foreach (Player player in alivePlayers) {
                    print(player.name);
                }
            }
        }
    }

    public List<Player> GetAlivePlayers() {
        List<Player> alivePlayers = new List<Player>();

        if (isLocalPlayer) {
            Player[] players = FindObjectsOfType<Player>();

            foreach (Player player in players) {
                if (player.IsAlive && !player.isLocalPlayer) { // If alive and not itself
                    alivePlayers.Add(player);
                }
            }

        }

        return alivePlayers;
    }
}
