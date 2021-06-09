using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public string Username;

    public const string MAIN_MENU_SCENE_NAME = "MainMenu";
    public const string LOBBY_SCENE_NAME = "Lobby";
    public const string GAME_SCENE_NAME = "Game";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}