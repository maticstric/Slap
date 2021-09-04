using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public string Username;

    public string LOBBY_SCENE_NAME;

    public string[] LEVEL_SCENE_NAMES;

    public string InitialLevel;

    private string _lastLevelSelected;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }

        InitialLevel = LEVEL_SCENE_NAMES[5];
        _lastLevelSelected = InitialLevel;

        DontDestroyOnLoad(gameObject);
    }

    public string GetRandomLevel() {
        int randomIndex = Random.Range(0, LEVEL_SCENE_NAMES.Length);
        string randomLevel = LEVEL_SCENE_NAMES[randomIndex];

        if (_lastLevelSelected != "") {
            while (randomLevel == _lastLevelSelected) { // Never go to the same level twice in a row
                randomIndex = Random.Range(0, LEVEL_SCENE_NAMES.Length);
                randomLevel = LEVEL_SCENE_NAMES[randomIndex];
            }
        }

        _lastLevelSelected = randomLevel;

        return randomLevel;
    }
}