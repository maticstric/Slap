using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {
    public enum Characters {
        Sphere,
        Donut,
        Cube,
        Pyramid
    };

    [Header("Objects")]
    [SerializeField] private ColorSelect colorSelect;
    [Space]
    [SerializeField] private GameObject sphereCharacter;
    [SerializeField] private GameObject donutCharacter;
    [SerializeField] private GameObject cubeCharacter;
    [SerializeField] private GameObject pyramidCharacter;
    [Space]
    [SerializeField] private Button sphereButton;
    [SerializeField] private Button donutButton;
    [SerializeField] private Button cubeButton;
    [SerializeField] private Button pyramidButton;

    public Characters CurrentCharacter;

    private void Awake() {
        CurrentCharacter = Characters.Sphere;
        MyNetworkManager.Instance.GamePlayerPrefab = sphereCharacter;

        sphereButton.onClick.AddListener(() => SelectCharacter(Characters.Sphere));
        donutButton.onClick.AddListener(() => SelectCharacter(Characters.Donut));
        cubeButton.onClick.AddListener(() => SelectCharacter(Characters.Cube));
        pyramidButton.onClick.AddListener(() => SelectCharacter(Characters.Pyramid));
    }

    public void SelectCharacter(Characters character) {
        if (character == Characters.Sphere) {
            MyNetworkManager.Instance.GamePlayerPrefab = sphereCharacter;
            CurrentCharacter = Characters.Sphere;

            colorSelect.SelectColor(colorSelect.CurrentColor); // Have to chose different texture, same color, for new character
        } else if (character == Characters.Donut) {
            MyNetworkManager.Instance.GamePlayerPrefab = donutCharacter;
            CurrentCharacter = Characters.Donut;

            colorSelect.SelectColor(colorSelect.CurrentColor);
        } else if (character == Characters.Cube) {
            MyNetworkManager.Instance.GamePlayerPrefab = cubeCharacter;
            CurrentCharacter = Characters.Cube;

            colorSelect.SelectColor(colorSelect.CurrentColor);
        } else if (character == Characters.Pyramid) {
            MyNetworkManager.Instance.GamePlayerPrefab = pyramidCharacter;
            CurrentCharacter = Characters.Pyramid;

            colorSelect.SelectColor(colorSelect.CurrentColor);
        }
    }
}
