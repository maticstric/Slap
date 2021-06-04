using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour {
    [Header("UI")]
    [SerializeField] private TMP_InputField usernameInputField;

    public void Login() {
        GameManager.Instance.Username = usernameInputField.text;

        ScreenManager.Instance.SwitchTo("MainMenu");
    }
}