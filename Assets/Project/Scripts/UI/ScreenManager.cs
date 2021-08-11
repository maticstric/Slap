using UnityEngine;
using System;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {
    public static ScreenManager Instance;

    public List<GameObject> Screens; // List of all screens (screens[0] should be what appears first in the game)
    public GameObject ActiveScreen;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }
    }

    private void Start() {
        SwitchTo(Screens[0].name);
    }

    public void SwitchTo(string screenName) {
        GameObject screen = FindScreen(screenName);

        if (ActiveScreen != null) {
            ActiveScreen.SetActive(false);
        }

        screen.SetActive(true);

        ActiveScreen = screen;
    }

    public GameObject FindScreen(string screenName) {
        GameObject screen = Array.Find(Screens.ToArray(), (s) => s != null && s.name == screenName);

        return screen;
    }
}