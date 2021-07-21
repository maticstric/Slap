using UnityEngine;
using Mirror;

public class PlayerDeath : NetworkBehaviour {
    private Player _player;
    private CameraFollow _cameraFollow;
    private Transform _spectatePosition;

    private void Awake() {
        _player = GetComponent<Player>();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _spectatePosition = GameObject.Find("SpectatePosition").transform;
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.name.Equals("DeathTrigger")) {
            if (isLocalPlayer) {
                _player.CmdSetIsAlive(false);

                // TODO: Display YOU DIED etc.

                _cameraFollow.MoveToSpectate(_spectatePosition.position, _spectatePosition.rotation);

            }

            if (isServer) {
                GameManager.Instance.CmdLoadRandomLevel();
            }
        }
    }

    //private List<Player> GetAlivePlayers() {
    //    List<Player> alivePlayers = new List<Player>();

    //    if (isLocalPlayer) {
    //        Player[] players = FindObjectsOfType<Player>();

    //        foreach (Player player in players) {
    //            if (player.IsAlive && !player.isLocalPlayer) { // If alive and not itself
    //                alivePlayers.Add(player);
    //            }
    //        }

    //    }

    //    return alivePlayers;
    //}
}
