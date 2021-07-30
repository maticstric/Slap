using Mirror;

public class DeathTrigger : NetworkBehaviour {
    //private void OnTriggerExit(Collider collider) {
    //    GameObject playerGameObject = collider.transform.parent.gameObject; // Collider is on child of player
    //    Player player = playerGameObject.GetComponent<Player>();

    //    if (playerGameObject.tag.Equals("Player")) {
    //        if (isServer) {
    //            if (CountAlivePlayers() == 2) {
    //                print("Level done");
    //                string randomLevel = GameManager.Instance.GetRandomLevel();
    //                MyNetworkManager.singleton.ServerChangeScene(randomLevel);
    //            }

    //            //player.CmdSetIsAlive(false);
    //        }
    //    }
    //}

    //private int CountAlivePlayers() {
    //    GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
    //    int count = 0;

    //    foreach (GameObject playerGameObject in playerGameObjects) {
    //        Player player = playerGameObject.GetComponent<Player>();

    //        print(playerGameObject.name);

    //        if (player.IsAlive) {
    //            count++;
    //        }
    //    }

    //    return count;
    //}
}
