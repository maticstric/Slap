using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private MyNetworkManager networkManager;

    [Header("UI")]
    [SerializeField] private TMP_InputField ipInputField;

    public void HostLobby() {
        networkManager.StartHost();
    }

    public void JoinLobby() {
        string ip = ipInputField.text;

        networkManager.networkAddress = ip;
        networkManager.StartClient();
    }
}
