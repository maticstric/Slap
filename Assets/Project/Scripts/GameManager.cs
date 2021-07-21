using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public string Username;

    public const string MAIN_MENU_SCENE_NAME = "MainMenu";
    public const string LOBBY_SCENE_NAME = "Lobby";
    public const string LEVEL01_SCENE_NAME = "Level01";
    public const string LEVEL02_SCENE_NAME = "Level02";

    string[] LEVEL_SCENE_NAMES = new string[] {
        "Level01", "Level02"
    };

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void CmdLoadRandomLevel() {
        string randomScene = GetRandomLevel();
        MyNetworkManager.singleton.ServerChangeScene(randomScene);
    }

    public string GetRandomLevel() {
        int randomIndex = Random.Range(0, LEVEL_SCENE_NAMES.Length);

        return LEVEL_SCENE_NAMES[randomIndex];
    }
}