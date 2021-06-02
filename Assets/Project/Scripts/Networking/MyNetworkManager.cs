using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager {
    public override void OnClientConnect(NetworkConnection conn) {
        ScreenManager.Instance.FindScreen("MainMenu").SetActive(false);
    }
}