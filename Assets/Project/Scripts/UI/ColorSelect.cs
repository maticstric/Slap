using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour {
    public enum Colors {
        Blue,
        Green,
        Red,
        Yellow
    };

    [Header("Objects")]
    [SerializeField] private CharacterSelect characterSelect;
    [Space]
    [SerializeField] private Texture[] sphereTextures; // Make sure these arrays are orderer correctly
    [SerializeField] private Texture[] donutTextures;  // Blue, Green, Red, Yellow
    [SerializeField] private Texture[] cubeTextures;
    [SerializeField] private Texture[] pyramidTextures;
    [Space]
    [SerializeField] private Button blueButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button yellowButton;

    public Colors CurrentColor;

    private void Awake() {
        CurrentColor = Colors.Red;
        MyNetworkManager.Instance.GamePlayerTexture = sphereTextures[(int)Colors.Red];

        blueButton.onClick.AddListener(() => SelectColor(Colors.Blue));
        greenButton.onClick.AddListener(() => SelectColor(Colors.Green));
        redButton.onClick.AddListener(() => SelectColor(Colors.Red));
        yellowButton.onClick.AddListener(() => SelectColor(Colors.Yellow));
    }

    public void SelectColor(Colors color) {
        if (characterSelect.CurrentCharacter == CharacterSelect.Characters.Sphere) {
            MyNetworkManager.Instance.GamePlayerTexture = sphereTextures[(int)color];
        } else if (characterSelect.CurrentCharacter == CharacterSelect.Characters.Donut) {
            MyNetworkManager.Instance.GamePlayerTexture = donutTextures[(int)color];
        } else if (characterSelect.CurrentCharacter == CharacterSelect.Characters.Cube) {
            MyNetworkManager.Instance.GamePlayerTexture = cubeTextures[(int)color];
        } else if (characterSelect.CurrentCharacter == CharacterSelect.Characters.Pyramid) {
            MyNetworkManager.Instance.GamePlayerTexture = pyramidTextures[(int)color];
        }
    }
}
