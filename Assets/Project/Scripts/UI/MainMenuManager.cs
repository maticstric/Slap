using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    [Header("UI")]
    [SerializeField] private TMP_InputField ipInputField;

    public void HostLobby() {
        MyNetworkManager.singleton.StartHost();
    }

    public void JoinLobby() {
        string ip = ipInputField.text;

        MyNetworkManager.singleton.networkAddress = ip;

        MyNetworkManager.singleton.StartClient();
    }
}
