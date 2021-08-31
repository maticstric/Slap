using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CharacterSelect : NetworkBehaviour {
    public static CharacterSelect Instance; // Need it to access GetTexture and GetCharacterPrefab globaly
                                            // Better than moving those to some random file
    public enum Characters {
        Sphere,
        Donut,
        Cube,
        Pyramid
    };

    public enum Colors {
        Blue,
        Green,
        Red,
        Yellow
    };

    [Header("Objects")]
    [SerializeField] private GameObject sphereCharacter;
    [SerializeField] private GameObject donutCharacter;
    [SerializeField] private GameObject cubeCharacter;
    [SerializeField] private GameObject pyramidCharacter;
    [Space]
    [SerializeField] private Button sphereButton;
    [SerializeField] private Button donutButton;
    [SerializeField] private Button cubeButton;
    [SerializeField] private Button pyramidButton;
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
    [Space]

    public Characters CurrentCharacter = Characters.Sphere;
    public Colors CurrentColor = Colors.Red;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);

            return;
        }
    }

    public override void OnStartClient() {
        CmdSelectCharacter(Characters.Sphere);
        CmdSelectColor(Colors.Red);

        sphereButton.onClick.AddListener(() => CmdSelectCharacter(Characters.Sphere));
        donutButton.onClick.AddListener(() => CmdSelectCharacter(Characters.Donut));
        cubeButton.onClick.AddListener(() => CmdSelectCharacter(Characters.Cube));
        pyramidButton.onClick.AddListener(() => CmdSelectCharacter(Characters.Pyramid));

        blueButton.onClick.AddListener(() => CmdSelectColor(Colors.Blue));
        greenButton.onClick.AddListener(() => CmdSelectColor(Colors.Green));
        redButton.onClick.AddListener(() => CmdSelectColor(Colors.Red));
        yellowButton.onClick.AddListener(() => CmdSelectColor(Colors.Yellow));

        base.OnStartClient();
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectCharacter(Characters character, NetworkConnectionToClient sender = null) {
        CurrentCharacter = character;

        MyNetworkManager.Instance.SetCharacter(character, sender);
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectColor(Colors color, NetworkConnectionToClient sender = null) {
        CurrentColor = color;

        MyNetworkManager.Instance.SetColor(color, sender);
    }

    public Texture GetTexture(Colors color, Characters character) {
        if (character == Characters.Sphere) {
            return sphereTextures[(int)color];
        } else if (character == Characters.Donut) {
            return donutTextures[(int)color];
        } else if (character == Characters.Cube) {
            return cubeTextures[(int)color];
        } else if (character == Characters.Pyramid) {
            return pyramidTextures[(int)color];
        }

        return null;
    }

    public GameObject GetCharacterPrefab(Characters character) {
        if (character == Characters.Sphere) {
            return sphereCharacter;
        } else if (character == Characters.Donut) {
            return donutCharacter;
        } else if (character == Characters.Cube) {
            return cubeCharacter;
        } else if (character == Characters.Pyramid) {
            return pyramidCharacter;
        }

        return null;
    }
}