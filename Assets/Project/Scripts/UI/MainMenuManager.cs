using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    [Header("UI")]
    [SerializeField] private TMP_InputField ipInputField;

    public void HostLobby() {
        MyNetworkManager.Instance.StartHost();
    }

    public void JoinLobby() {
        string ip = ipInputField.text;

        MyNetworkManager.Instance.networkAddress = ip;
        MyNetworkManager.Instance.StartClient();
    }
}
