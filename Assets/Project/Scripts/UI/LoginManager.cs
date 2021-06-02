using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private GameManager gameManager;

    [Header("UI")]
    [SerializeField] private TMP_InputField usernameInputField;

    public void Login() {
        gameManager.Username = usernameInputField.text;

        ScreenManager.Instance.SwitchTo("MainMenu");
    }
}