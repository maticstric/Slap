using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public string Username;

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